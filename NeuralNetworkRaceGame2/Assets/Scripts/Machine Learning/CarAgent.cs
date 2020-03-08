using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class CarAgent : Agent
{
  private GameObject nextCheckpoint;

  //Components
  private Rigidbody rBody;
  private CheckpointHandler checkpointHandler;
  private CarController0 carController;
  private TimeBetweenCheckpoints checkpointTimer;

  private float lookingDir;
  private float reward;
  private float games;

  void Start()
  {
    //Initializing components
    carController = gameObject.GetComponent<CarController0>();
    rBody = gameObject.GetComponent<Rigidbody>();
    checkpointHandler = gameObject.GetComponent<CheckpointHandler>();
    checkpointTimer = gameObject.GetComponent<TimeBetweenCheckpoints>();
    nextCheckpoint = checkpointHandler.NextCheckpoint();

    reward = 0;
    games = 0;
  }

  public override void CollectObservations()
  {
    //Updating
    List<RaycastHit> sensors = carController.GetSensors();
    lookingDir = carController.GetLookingDir();

    //Checkpoint and Car positions
    AddVectorObs(nextCheckpoint.transform.position);
    AddVectorObs(gameObject.transform.position);
    AddVectorObs(carController.GetSpeed());
    AddVectorObs(lookingDir);

    //Agent velocity
    AddVectorObs(rBody.velocity.x);
    AddVectorObs(rBody.velocity.z);

    //Sensors
    AddVectorObs(carController.GetNormSensors(0));
    AddVectorObs(carController.GetNormSensors(1));
    AddVectorObs(carController.GetNormSensors(2));
    AddVectorObs(carController.GetNormSensors(3));
    AddVectorObs(carController.GetNormSensors(4));


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
    gameObject.GetComponent<CollisionHandler>().ResetCar(); //resets the car from external script
    checkpointHandler.ResetCheckpoints();
    nextCheckpoint = checkpointHandler.NextCheckpoint();
    games += 1;

  }

  public override void AgentAction(float[] vectorAction)
  {
    List<RaycastHit> sensors = carController.GetSensors();

    Vector3 controlSignal = Vector3.zero;
    controlSignal.z = vectorAction[0];
    controlSignal.x = vectorAction[1];
    carController.Steer(controlSignal.z);
    carController.Drive(controlSignal.x);

    //on screen monitoring
    Monitor.Log("Steering", controlSignal.z);
    Monitor.Log("Driving", controlSignal.x);

    Monitor.Log("Front", (carController.GetNormSensors(0)));
    Monitor.Log("Left", (carController.GetNormSensors(1)));
    Monitor.Log("Right", (carController.GetNormSensors(2)));
    Monitor.Log("diaLeft", (carController.GetNormSensors(3)));
    Monitor.Log("diaRight", (carController.GetNormSensors(4)));

    Monitor.Log("LookingDir", "" + lookingDir);

    Monitor.Log("Reward", "" + reward);
    Monitor.Log("Games", "" + games);
    Monitor.Log("Speed", "" + carController.GetSpeed());
    Monitor.Log("Time Checkpoint", "" + checkpointTimer.GetTimer());

    //small reward for looking the right direction and getting higher with speed 
    if (lookingDir > .90f)
    {
      AddMyReward(0.0005f * carController.GetSpeed());
      Debug.Log("Reward added (Direction)");
    }
    //Reset agent if next checkpoint isn't reached in 1 Minute
    if (checkpointTimer.GetTimer() > checkpointTimer.resetTime)
    {
      checkpointTimer.ResetTimer();
      Done();
    }

    TimePunishment();
  }

  private void OnCollisionEnter(Collision collision)
  {
    if (collision.gameObject.tag == "wall")
    {
      AddMyReward(-1.0f);
      Debug.Log("Reward removed (Wall)");
      checkpointHandler.ResetCheckpoints();
      Done();
    }
  }
  private void OnTriggerEnter(Collider other)
  {
    if (other.gameObject == nextCheckpoint)
    {
      AddMyReward(.5f);
      //updating next checkpoint
      nextCheckpoint = checkpointHandler.CheckCheckpoint(other);
      Debug.Log("Reward added (Checkpoint)");
    }
  }

  private void TimePunishment()
  {
    AddMyReward(-.001f);
  }

  //functions to keep track in realtime of the reward
  private void AddMyReward(float amount)
  {
    AddReward(amount);
    reward += amount;
  }
}
