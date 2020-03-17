using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class CarAgent : Agent
{
  public float mapMaxX;
  public float mapMaxZ;

  private GameObject nextCheckpoint;
  //Components
  private Rigidbody rBody;
  private CarController0 carController;
  private EnviormentHandler enviormentHandler; 

  private float lookingDir;
  private float reward;
  private float games;

  void Start()
  {
    //Initializing components
    carController = gameObject.GetComponent<CarController0>();
    rBody = gameObject.GetComponent<Rigidbody>();
    enviormentHandler = gameObject.GetComponent<EnviormentHandler>();

    reward = 0;
    games = 0;
  }

  public override void CollectObservations()
  {
    //Updating
    List<RaycastHit> sensors = carController.GetSensors();
    lookingDir = carController.GetLookingDir();

    //Checkpoint and Car positions
    AddVectorObs(gameObject.transform.position.x/mapMaxX);
    AddVectorObs(gameObject.transform.position.z / mapMaxZ);
    AddVectorObs(nextCheckpoint.transform.position.x /mapMaxX);
    AddVectorObs(nextCheckpoint.transform.position.z / mapMaxZ);
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

  public override void AgentAction(float[] vectorAction)
  {
    Vector3 controlSignal = Vector3.zero;
    controlSignal.z = vectorAction[0];
    controlSignal.x = vectorAction[1];
    carController.Steer(controlSignal.z);
    carController.Drive(controlSignal.x);

    //on screen monitoring of values
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
    Monitor.Log("Time Checkpoint", "" + enviormentHandler.GetTimerCheckpoint());
    Monitor.Log("Time Game", "" + enviormentHandler.GetTimerGame());
    Monitor.Log("MapZ", gameObject.transform.position.z / mapMaxZ);
    Monitor.Log("MapX", gameObject.transform.position.x / mapMaxX);

    //small reward for looking the right direction and getting higher with speed 
    if (lookingDir > .90f && (carController.GetSpeed() >= 1))
    {
      AddMyReward(0.0005f * carController.GetSpeed());
      Debug.Log("Reward added (Direction)");
    }
    else
    {
      TimePunishment();
    }

    //Reset agent if next checkpoint isn't reached in 1 Minute
    if (enviormentHandler.GetTimerCheckpoint() > enviormentHandler.resetTimer)
    {
      enviormentHandler.ResetTimerCheckpoint();
      Done();
    }
  }

  public override void AgentReset()
  {
    carController.ResetCar();
    games += 1;
    enviormentHandler.ResetCheckpoints();
    UpdateCheckpoint();
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

  private void OnCollisionEnter(Collision collision)
  {
    if (collision.gameObject.tag == "wall")
    {
      AddMyReward(-1.0f);
      Debug.Log("Reward removed (Wall)");
      enviormentHandler.ResetCheckpoints();
      Done();
    }
  }
  private void OnTriggerEnter(Collider other)
  {
    if (other.gameObject == nextCheckpoint)
    {
      AddMyReward(.5f);
      //updating next checkpoint
      enviormentHandler.CheckCheckpoint(other);
      UpdateCheckpoint();
      Debug.Log("Reward added (Checkpoint)");
    }
  }

  private void UpdateCheckpoint()
  {
    nextCheckpoint = enviormentHandler.NextCheckpoint();
    carController.nextCheckpoint = enviormentHandler.NextCheckpoint();
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
