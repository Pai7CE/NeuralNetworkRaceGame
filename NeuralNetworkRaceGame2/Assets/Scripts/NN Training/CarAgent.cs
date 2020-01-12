using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class CarAgent : Agent
{
    public GameObject wall;

    private Vector3 resetPosition;
    private Quaternion resetAngle;
    private Rigidbody rBody;

    
    private NNCarController carController;

    void Start()
    {
        
        carController = gameObject.GetComponent<NNCarController>();
    }

    public override void CollectObservations()
    {
        
    }

    //For manual control
    public override float[] Heuristic() 
    {
        var action = new float[2];
        //steering
        action[0] = Input.GetAxis("Horizontal");
        //Accelaration & Brakes
        action[1] = Input.GetAxis("Vertical");
        return action;
    }

    public override void AgentReset()
    {
        Debug.Log("reset");
        gameObject.GetComponent<CollisionWall>().resetCar(); //resets the car from external script

    }

    public override void AgentAction(float[] vectorAction)
    {
        Vector3 controlSignal = Vector3.zero;
        controlSignal.z = vectorAction[0];
        controlSignal.x = vectorAction[1];
        carController.Steer(controlSignal.z);
        carController.Drive(controlSignal.x);

        Monitor.Log("Steering", controlSignal.z);
        Monitor.Log("Driving", controlSignal.x);
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject == wall)
        {
            Done();
        }
    }

}
