using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCollissionWall : MonoBehaviour
{
    public bool detectCollision = false;
    public GameObject car;
    public GameObject wall;

    private Vector3 startPosition;
    private Quaternion startRotation;
    private Rigidbody rbCar;


    private void Start()
    { //filling needed variables
        startPosition = car.transform.position;
        startRotation = car.transform.rotation;
        rbCar = car.GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("collision detected");
        Debug.Log(collision.gameObject.name);
        if (detectCollision)
        {
            if(collision.gameObject == wall) 
            {
                car.transform.position = startPosition;
                car.transform.rotation = startRotation;
                rbCar.velocity = new Vector3(0, 0, 0);
                
            }
        }
    }
}
