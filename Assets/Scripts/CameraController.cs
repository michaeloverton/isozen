using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera playerCamera;
    bool dragging = false;
    public float mouseHorizontalSpeed = 200f;
    public float mouseVerticalSpeed = 200f;
    public float mobileHorizontalSpeed = 100f;
    public float mobileVerticalSpeed = 100f;
    public float zoomSpeed = 1000f;
    private Vector3 cameraFocus = new Vector3(0,0,0);
    private bool splitting;
    private Ray splittingRay;
    public Transform touchLocation;
    public bool canMove = false;

    void Update() {
        // transform.rotation = transform.rotation * Quaternion.AngleAxis(5, Vector3.up);

        if(canMove) {
            // Handle mouse inputs.
            if(Input.GetMouseButton(0)) {
                // float horizontalRotation = mouseHorizontalSpeed * Input.GetAxis("Mouse X");
                // transform.Translate(horizontalRotation * Vector3.left * Time.deltaTime);
                
                // Quaternion horizontalRotation = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * mouseHorizontalSpeed * Time.deltaTime, Vector3.up);
                // transform.position = horizontalRotation * transform.position;

                // transform.RotateAround(cameraFocus, Vector3.up, mouseHorizontalSpeed * Input.GetAxis("Mouse X") * Time.deltaTime);
                // transform.RotateAround(cameraFocus, Vector3.left, mouseVerticalSpeed * Input.GetAxis("Mouse Y") * Time.deltaTime);


                transform.rotation = transform.rotation * Quaternion.AngleAxis(mouseHorizontalSpeed * Input.GetAxis("Mouse X") * Time.deltaTime, Vector3.up);
                transform.rotation = transform.rotation * Quaternion.AngleAxis(mouseVerticalSpeed * Input.GetAxis("Mouse Y") * Time.deltaTime, Vector3.left);
                
                // TRY THIS
                // Quaternion.AngleAxis()

                // float verticalRotation = mouseVerticalSpeed * Input.GetAxis("Mouse Y");
                // transform.Translate(verticalRotation * Vector3.down * Time.deltaTime);

                // transform.RotateAround(cameraFocus, Vector3.left, mouseVerticalSpeed * Input.GetAxis("Mouse Y") * Time.deltaTime);
                
                // Handle splitting.
                Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
                splittingRay =  ray;
                touchLocation.transform.position = ray.origin;
                splitting = true;
            } else {
                splitting = false;
            }

            float mouseScroll = Input.GetAxis("Mouse ScrollWheel");
            if(mouseScroll > 0 || mouseScroll < 0) {
                playerCamera.orthographicSize -= mouseScroll * zoomSpeed * Time.deltaTime;
            }

            // Handle touch inputs.
            if (Input.touchCount ==1) {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Moved) {
                    // float horizontalRotation = mobileHorizontalSpeed * touch.deltaPosition.x;
                    // transform.Translate(horizontalRotation * Vector3.left * Time.deltaTime);
                    transform.RotateAround(cameraFocus, Vector3.up, mobileHorizontalSpeed * touch.deltaPosition.x * Time.deltaTime);

                    // float verticalRotation = mobileVerticalSpeed * touch.deltaPosition.y;
                    // transform.Translate(verticalRotation * Vector3.down * Time.deltaTime);
                } else if(touch.phase == TouchPhase.Stationary) {
                    // Handle splitting.
                    Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
                    splittingRay =  ray;
                    touchLocation.transform.position = ray.origin;
                    splitting = true;
                } else {
                    splitting = false;
                }
            }

            // Handle zooming. Stolen code.
            if (Input.touchCount == 2){
                // get current touch positions
                Touch tZero = Input.GetTouch(0);
                Touch tOne = Input.GetTouch(1);
                // get touch position from the previous frame
                Vector2 tZeroPrevious = tZero.position - tZero.deltaPosition;
                Vector2 tOnePrevious = tOne.position - tOne.deltaPosition;

                float oldTouchDistance = Vector2.Distance (tZeroPrevious, tOnePrevious);
                float currentTouchDistance = Vector2.Distance (tZero.position, tOne.position);

                // get offset value
                float deltaDistance = oldTouchDistance - currentTouchDistance;
                
                playerCamera.orthographicSize += deltaDistance * zoomSpeed;
                // set min and max value of Clamp function upon your requirement
                playerCamera.orthographicSize = Mathf.Clamp(playerCamera.orthographicSize, 25, 800);
            }
        }
    }

    // void LateUpdate() {
    //     transform.LookAt(cameraFocus);
    // }

    public bool isSplitting() {
        return splitting;
    }

    public Transform getTouchLocation() {
        return touchLocation;
    }

    public Vector3 getSplittingVector() {
        // Just use a really big number here to make sure the vector is long enough.
        return 5000 * splittingRay.direction;
    }

    public void setCanMove(bool move) {
        canMove = move;
    }
}
