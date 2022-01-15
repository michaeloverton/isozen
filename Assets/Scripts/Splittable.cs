using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Splittable : MonoBehaviour
{
    private CameraController controller;
    public float reboundSpeed = 0.5f;
    private Vector3 originalPosition;
    public float positionTolerance = 0.1f;
    private float repulsionIntensity = 30f;
    private float maxRepulsionDistance = 150f;

    // Start is called before the first frame update
    void Start()
    {
        originalPosition = transform.position;

        // controller = GameObject.Find("Isometric Camera").GetComponent<CameraController>();
        // if(controller == null) {
        //     throw new System.Exception("failed to find controller");
        // }
    }

    void FixedUpdate()
    {
        // if(controller.isSplitting()) {

        //     Vector3 splittingVector = controller.getSplittingVector();
        //     Vector3 touchLocationToSplittingObject = transform.position - controller.getTouchLocation().position;
        //     Vector3 closestPoint = Vector3.Project(touchLocationToSplittingObject, splittingVector);
        //     Vector3 normal = touchLocationToSplittingObject - closestPoint;

        //     // Maybe remove the max repulsion concept since it might feel better without.
        //     float normalMagnitude = Vector3.Magnitude(normal);
        //     if(normalMagnitude < maxRepulsionDistance) {
        //         float inverseNormalMagnitude = 1/Mathf.Pow(normalMagnitude, 1);
        //         transform.Translate(repulsionIntensity * inverseNormalMagnitude * normal * Time.fixedDeltaTime, Space.World);

        //     } else if(Vector3.Distance(originalPosition, transform.position) > positionTolerance) {
        //         returnToOriginalPosition();
        //     }

        // } else {
        //     // Otherwise, move the repelled object back towards its original position.
        //     if(Vector3.Distance(originalPosition, transform.position) > positionTolerance) {
        //         returnToOriginalPosition();
        //     }
        // }
    }

    void returnToOriginalPosition() {
        Vector3 returnVector = originalPosition - transform.position;
        transform.Translate(reboundSpeed * returnVector * Time.fixedDeltaTime, Space.World);
    }
}
