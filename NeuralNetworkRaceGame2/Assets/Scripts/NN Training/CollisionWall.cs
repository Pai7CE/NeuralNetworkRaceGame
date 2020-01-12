using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionWall : MonoBehaviour
{

    private Rigidbody rBody;
    private Vector3 resetPosition;
    private Quaternion resetAngle;

    // initializing start position and Rigid Body of the car
    void Start() 
    {
        rBody = gameObject.GetComponent<Rigidbody>();
        resetPosition = gameObject.transform.position;
        resetAngle = gameObject.transform.rotation;
    }

    public void resetCar()
    {
        rBody.transform.localPosition = resetPosition;
        rBody.angularVelocity = Vector3.zero;
        rBody.velocity = Vector3.zero;
        rBody.transform.rotation = resetAngle;
    }
}
