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
    private TimeBetweenCheckpoints timeCheckpoints;

    private float dist;
    private float lookingDir;
    private float reward;
    private float games;

    void Start()
    {
        //Initializing components
        carController = gameObject.GetComponent<NNCarController>();
        checkpoint = gameObject.GetComponent<Checkpoint>();
        rBody = gameObject.GetComponent<Rigidbody>();         
        nextCheckpoint = checkpoint.nextCheckpoint();
        reducedDistance = gameObject.GetComponent<ReducedDistance>();
        timeCheckpoints = gameObject.GetComponent<TimeBetweenCheckpoints>();

        reward = 0;
        games = 0;
    }

    public override void CollectObservations()
    {
        //Updating
        List<RaycastHit> sensors = carController.getSensors();
        lookingDir = carController.getLookingDir();

        //Checkpoint and Car positions
        AddVectorObs(nextCheckpoint.transform.position);
        AddVectorObs(gameObject.transform.position);
        AddVectorObs(carController.getSpeed());

        //Agent velocity
        AddVectorObs(rBody.velocity.x);
        AddVectorObs(rBody.velocity.z);

        //Sensors
        AddVectorObs(carController.getNormSensors(0));
        AddVectorObs(carController.getNormSensors(1));
        AddVectorObs(carController.getNormSensors(2));
        AddVectorObs(carController.getNormSensors(3));
        AddVectorObs(carController.getNormSensors(4));

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
        checkpoint.resetCheckpoints();
        nextCheckpoint = checkpoint.nextCheckpoint();
        games += 1;

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

        Monitor.Log("Front", (carController.getNormSensors(0)));
        Monitor.Log("Left", (carController.getNormSensors(1)));
        Monitor.Log("Right", (carController.getNormSensors(2)));
        Monitor.Log("diaLeft", (carController.getNormSensors(3)));
        Monitor.Log("diaRight", (carController.getNormSensors(4)));

        Monitor.Log("LookingDir", "" + lookingDir);

        Monitor.Log("Reward", "" + reward);
        Monitor.Log("Games", "" + games);
        Monitor.Log("Speed", "" + carController.getSpeed());
        Monitor.Log("Time Checkpoint", "" + timeCheckpoints.getTimer());

        //small reward for looking the right direction and getting higher with speed 
        if (lookingDir > .8f)
        {
            addReward(0.0001f);
            addReward(0.0005f * carController.getSpeed());
            //Debug.Log("Reward added (Direction)");
        }
        //Reset agent if next checkpoint isn't reached in 1 Minute
        if(timeCheckpoints.getTimer() > 60f)
        {
            timeCheckpoints.resetTimer();
            Done();
        }

        timePunishment();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject == wall)
        {
            addReward(-1.0f);
            Debug.Log("Reward removed (Wall)");
            checkpoint.resetCheckpoints();
            Done();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == nextCheckpoint)
        {
            addReward(.5f);
            //updating next checkpoint
            nextCheckpoint = checkpoint.checkCheckpoint(other);
            Debug.Log("Reward added (Checkpoint)");
        }
    }

    private void timePunishment()
    {
        addReward(-.001f);
    }

    //functions to keep track in realtime of the reward
    private void addReward(float amount)
    {
        AddReward(amount);
        reward += amount;
    }
}
