using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Cameras;

public class Generator : MonoBehaviour
{
    public CustomFreeLookCamera freeLookCamera;
    public List<GameObject> rooms;
    public LightController lightController;
    int currentRooms = 0;
    public bool debug = false;
    private List<int> randomizedRoomIndexes = new List<int>();
    private bool randomScalingOn = false;
    private int complexity = 0;


    void Start()
    {
        // Randomize the lights.
        lightController.NewDirectionLightColors();
        lightController.NewPointLightColors();

        // Create a further randomization structure so that not all rooms have equal probability of creation.
        for(int roomIndex=0; roomIndex<rooms.Count; roomIndex++) {
            int randomCount = Random.Range(0, 50);
            for(int i=0; i<randomCount; i++) {
                randomizedRoomIndexes.Add(roomIndex);
            }
        }

        Debug.Log("total random length: " + randomizedRoomIndexes.Count);

        if(Random.Range(0,2) == 1) {
            Debug.Log("random scaling on");
            randomScalingOn = true;
        }

        // Set the complexity, which has not been destroyed from the menu scene.
        Complexity c = GameObject.FindObjectOfType<Complexity>();
        // If we can't find it, just set complexity to default value.
        if(c != null) {
            complexity = c.getComplexity();
        } else {
            complexity = 500;
        }

        StartCoroutine(build());
    }

    IEnumerator build() {
        yield return null;

        List<GameObject> allRooms = new List<GameObject>();

        // We store all connections in order to delete them later.
        List<GameObject> allConnections = new List<GameObject>();

        GameObject structure = new GameObject("Structure");

        // Create start room.
        int startRoomIndex = Random.Range(0, rooms.Count);
        GameObject start = Instantiate(rooms[startRoomIndex], new Vector3(0,0,0), Quaternion.identity);
        allRooms.Add(start);

        // Add start to the total structure.
        start.transform.parent = structure.transform;

        // Get all connections leaving start room.
        List<GameObject> connections = start.GetComponent<RoomConnections>().connections;

        // Use the queue for breadth first creation.
        Queue<GameObject> allExits = new Queue<GameObject>();
        foreach(GameObject connection in connections) {
            allExits.Enqueue(connection);
            allConnections.Add(connection);
        }

        while(currentRooms <= complexity) {
            yield return null;

            NewRoom:

            Log("creating new room: " + currentRooms);

            // Get the exit of the room we built previously, if any exits exist in queue.
            if(allExits.Count == 0) {
                Log("no exits to dequeue. ending");
                break;
            }
            GameObject exit = allExits.Dequeue();
            Vector3 exitPosition = exit.GetComponent<Connection>().connectionPoint.position;
            Vector3 exitNormal = exit.GetComponent<Connection>().exitPlane.transform.up;
            Log("attaching room to connection: " + exit.GetComponent<Connection>().name + " of " + exit.GetComponentInParent<RoomConnections>().gameObject.name);

            // We will track which room indexes we have already tried to build. Start at a random one.
            // int firstRoomIndexToTry = Random.Range(0, rooms.Count-1);
            int firstRoomIndexToTry = randomizedRoomIndexes[Random.Range(0, randomizedRoomIndexes.Count)];
            int currentRoomIndexToTry = firstRoomIndexToTry;

            CreateRoom:

            // Instantiate the random room.
            GameObject roomToBuild = rooms[currentRoomIndexToTry];
            Log("attempting to build room: " + roomToBuild.ToString());

            GameObject room = Instantiate(roomToBuild, new Vector3(0,0,0), Quaternion.identity);
            room.name = "room" + currentRooms;
            room.SetActive(false); // Disable room until it is successfully placed.

            // Randomly scale the rooms if enabled.
            float scalingFactor = 1;
            if(randomScalingOn) {
                scalingFactor = Random.Range(0.25f,2);
                room.transform.localScale = scalingFactor * room.transform.localScale;
            }

            RoomConnections roomComponent = room.GetComponent<RoomConnections>();

            // Start with a random entrance index and increment from there.
            List<GameObject> roomConnections = roomComponent.connections;
            int firstConnectionIndexToTry = Random.Range(0, roomConnections.Count);
            int currentConnectionIndex = firstConnectionIndexToTry;

            // Add all connections from this room to our total list of connections.
            foreach(GameObject c in roomConnections) {
                allConnections.Add(c);
            }

            ChooseEntrance:
            
            // Ensure that room position is at origin before we move it into position.
            // It may have moved from origin if we have previously tried a different connection as entrance.
            room.transform.position = new Vector3(0,0,0);
            room.transform.rotation = Quaternion.identity;

            Log("trying to use as entrance connection index: " + currentConnectionIndex);
            GameObject entranceConnection = roomConnections[currentConnectionIndex];

            // Translate room into position.
            Vector3 entrancePosition = entranceConnection.GetComponent<Connection>().connectionPoint.position;
            room.transform.Translate(exitPosition - entrancePosition, Space.Self);

            // Rotate the room by rotating the angle between previous exit normal and new entrance normal.
            Vector3 newEntrancePosition = roomComponent.connections[currentConnectionIndex].GetComponent<Connection>().connectionPoint.position;
            Vector3 newEntranceNormal = roomComponent.connections[currentConnectionIndex].GetComponent<Connection>().entrancePlane.transform.up;
            float rotation = Vector3.SignedAngle(newEntranceNormal, exitNormal, Vector3.up);
            Log("rotating " + room.name + " " + rotation + " about y axis");
            room.transform.RotateAround(newEntrancePosition, Vector3.up, rotation);

            // Check if new room intersects any previous rooms.
            List<Collider> colliders = roomComponent.GetCollisions(scalingFactor);
            if(colliders.Count > 0) {
                Log("room is colliding with " + colliders.Count + " other rooms");
                currentConnectionIndex = (currentConnectionIndex + 1) % roomConnections.Count;

                if(currentConnectionIndex != firstConnectionIndexToTry) {
                    Log("choosing new entrance to try");
                    goto ChooseEntrance;

                } else {
                    Destroy(room);

                    currentRoomIndexToTry = (currentRoomIndexToTry + 1) % rooms.Count;

                    if(currentRoomIndexToTry != firstRoomIndexToTry) {
                        Log("trying to add another room, current count: " + currentRooms);
                        // If we have other rooms we could add, try them.
                        goto CreateRoom;
                    } else {
                        // Otherwise, create a new room.
                        Log("no more connections to try, adding a new room new position, current count: " + currentRooms);
                        goto NewRoom;
                    }
                    
                }
            }
            
            // Room was successfully created and does not intersect.
            currentRooms++;

            Log("no collisions. adding room to allrooms"); 
            allRooms.Add(room);

            // Add all non-entrance connections into the exit queue.
            for(int i=0; i < roomConnections.Count; i++) {
                if(i != currentConnectionIndex) {
                    allExits.Enqueue(roomConnections[i]);
                }
            }

            // Add the room to the total structure.
            room.transform.parent = structure.transform;

            // Activate room since it has been successfully placed.
            room.SetActive(true);
        }

        Log(allRooms.Count + " rooms created");

        // Allow the controller to move.
        // controller.setCanMove(true);

        // Set the target to be the center of the created structure.
        freeLookCamera.SetTarget(structure.transform);

        // Delete all of the connection GameObjects.
        foreach(GameObject c in allConnections) {
            Destroy(c);
        }
    }

    void Log(string message) {
        if(debug) {
            Debug.Log(message);
        }
    }
}

