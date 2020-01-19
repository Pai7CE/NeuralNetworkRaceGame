using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class CarAgent : Agent
{
    public GameObject wall;

    
    private Vector3 resetPosition;
    private Quaternion resetAngle;
    private GameObject nextCheckpoint;
    
    //Components
    private Rigidbody rBody;
    private Checkpoint checkpoint;
    private NNCarController carController;
    private ReducedDistance reducedDistance;

    private float dist;
    private float lookingDir;
    void Start()
    {
        //Initializing components
        carController = gameObject.GetComponent<NNCarController>();
        checkpoint = gameObject.GetComponent<Checkpoint>();
        rBody = gameObject.GetComponent<Rigidbody>();         
        nextCheckpoint = checkpoint.nextCheckpoint();
        reducedDistance = gameObject.GetComponent<ReducedDistance>();
    }

    public override void CollectObservations()
    {
        //Updating
        List<RaycastHit> sensors = carController.getSensors();
        lookingDir = carController.getLookingDir();

        //Checkpoint and Car positions
        AddVectorObs(nextCheckpoint.transform.localPosition);
        AddVectorObs(gameObject.transform.localPosition);

        //Agent velocity
        AddVectorObs(rBody.velocity.x);
        AddVectorObs(rBody.velocity.z);

        //Sensors
        AddVectorObs(normalize(sensors[0]));
        AddVectorObs(normalize(sensors[1]));
        AddVectorObs(normalize(sensors[2]));

        AddVectorObs(lookingDir);
        
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
        gameObject.GetComponent<CollisionWall>().resetCar(); //resets the car from external script

    }

    public override void AgentAction(float[] vectorAction)
    {
        List<RaycastHit> sensors = carController.getSensors();

        Vector3 controlSignal = Vector3.zero;
        controlSignal.z = vectorAction[0];
        controlSignal.x = vectorAction[1];
        carController.Steer(controlSignal.z);
        carController.Drive(controlSignal.x);

        //on screen monitoring
        Monitor.Log("Steering", controlSignal.z);
        Monitor.Log("Driving", controlSignal.x);

        Monitor.Log("Front", (normalize(sensors[0])));
        Monitor.Log("Left", (normalize(sensors[1])));
        Monitor.Log("Right", (normalize(sensors[2])));

        Monitor.Log("LookingDir", lookingDir);

        //small reward for looking the right direction
        if ((lookingDir > .94f) && (reducedDistance.distanceReducedToCheckpoint())) 
        {
            AddReward(0.1f);
        }
        //if distance towards next checkpoint reduced agent gets rewarded
        if (!reducedDistance.distanceReducedToCheckpoint())
        {
            AddReward(-0.01f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject == wall)
        {
            AddReward(-.5f);
            checkpoint.resetCheckpoints();
            Done();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == nextCheckpoint)
        {
            AddReward(1.0f);
            //updating next checkpoint
            nextCheckpoint = checkpoint.nextCheckpoint();
        }
    }

    private float normalize(RaycastHit rchit)
    {
        float normLength;
        if (rchit.distance == 0)
        {
            normLength = 1;
        }
        else
        {
            normLength = rchit.distance / carController.sensorLength;
        }
        return normLength;
    }

}
