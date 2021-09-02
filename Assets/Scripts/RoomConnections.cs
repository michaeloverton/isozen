using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomConnections : MonoBehaviour
{
    public bool isStartRoom = false;
    public List<GameObject> connections;
    // We use a Renderer instead of Collider because Colliders seemed to be translating incorrectly.
    public List<Renderer> boundingBoxes;

    // GetCollisions gets all the OTHER colliders that collide with this piece. It omits its own collider from the list.
    public List<Collider> GetCollisions(float scalingFactor) {
        BoxCollider boundingBox = GetComponent<BoxCollider>();
        // We need to create the overlap box while taking into account the rotation of the bounding box (hence transform.rotation * boundingBox.center)
        Collider[] colliders = Physics.OverlapBox(transform.position + (transform.rotation * (scalingFactor * boundingBox.center)), (scalingFactor * boundingBox.size)/2, transform.rotation);

        /*
        // Create temp test cube to try to match the overlap box.
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.position = transform.position + (transform.rotation * boundingBox.center);
        cube.transform.localScale = boundingBox.size;
        cube.transform.rotation = transform.rotation;
        cube.name = gameObject.name + " collision tester";
        cube.SetActive(false);
        */
        
        // Omit the collider of this room itself from the list of colliders that we are colliding with.
        List<Collider> actualColliders = new List<Collider>();
        foreach(Collider collider in colliders) {
            if(collider.gameObject.GetInstanceID() != gameObject.GetInstanceID()) {
                actualColliders.Add(collider);
            }
        }
        return actualColliders;
    }
}
