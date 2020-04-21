using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class CarAgent : Agent
{
  [Header("Evaluation")]
  public bool addInputs = false;
  public bool addOutputs = false;

  [Header("Map")]
  public float mapMaxX;
  public float mapMaxZ;

  private GameObject nextCheckpoint;
  //Components
  private CarController carController;
  private EnviormentHandler enviormentHandler; 

  private float lookingDir;
  private int games;

  void Start()
  {
    //Initializing components
    carController = gameObject.GetComponent<CarController>();
    enviormentHandler = gameObject.GetComponent<EnviormentHandler>();
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
    AddVectorObs(nextCheckpoint.transform.position.x / mapMaxX);
    AddVectorObs(nextCheckpoint.transform.position.z / mapMaxZ);
    AddVectorObs(carController.GetSpeed());
    AddVectorObs(lookingDir);

    //Sensors
    AddVectorObs(carController.GetNormSensors(0));
    AddVectorObs(carController.GetNormSensors(1));
    AddVectorObs(carController.GetNormSensors(2));
    AddVectorObs(carController.GetNormSensors(3));
    AddVectorObs(carController.GetNormSensors(4));
    if (addInputs)
    {
      AddVectorObs(carController.GetNormSensors(5));
      AddVectorObs(carController.GetNormSensors(6));
    }
  }

  public override void AgentAction(float[] vectorAction)
  {
    Vector3 controlSignal = Vector3.zero;
    
    controlSignal.z = vectorAction[0];
    controlSignal.x = vectorAction[1];

    if (addOutputs)
    {
      controlSignal.y = vectorAction[2];
      carController.Steer(controlSignal.z);
      carController.Accelerate(controlSignal.x);
      carController.Brake(controlSignal.y);

      Monitor.Log("Steering", controlSignal.z);
      Monitor.Log("Acceleration", controlSignal.x);
      Monitor.Log("Break", controlSignal.y);
    }
    else
    {
      carController.Steer(controlSignal.z);
      carController.Drive(controlSignal.x);

      Monitor.Log("Steering", controlSignal.z);
      Monitor.Log("Driving", controlSignal.x);
    }

    //on screen monitoring of values

    Monitor.Log("Front", (carController.GetNormSensors(0)));
    Monitor.Log("Left", (carController.GetNormSensors(1)));
    Monitor.Log("Right", (carController.GetNormSensors(2)));
    Monitor.Log("diaLeft", (carController.GetNormSensors(3)));
    Monitor.Log("diaRight", (carController.GetNormSensors(4)));

    if (addInputs)
    {
      Monitor.Log("diaLeft2", (carController.GetNormSensors(5)));
      Monitor.Log("diaRight2", (carController.GetNormSensors(6)));
    }

    Monitor.Log("LookingDir", "" + lookingDir);

    Monitor.Log("Games", "" + games);
    Monitor.Log("Speed", "" + carController.GetSpeed());
    Monitor.Log("Time Checkpoint", "" + enviormentHandler.GetTimerCheckpoint());
    Monitor.Log("Time Game", "" + enviormentHandler.GetTimerGame());
    Monitor.Log("Timer Round", ""+ enviormentHandler.GetTimerRound());
    Monitor.Log("MapZ", gameObject.transform.position.z / mapMaxZ);
    Monitor.Log("MapX", gameObject.transform.position.x / mapMaxX);

    //small reward for looking the right direction and getting higher with speed 
    if (lookingDir > .90f && (carController.GetSpeed() >= 1))
    {
      AddReward(0.0005f * carController.GetSpeed());
      Debug.Log("Reward added (Direction)");
    }
    else
    {
      NoMovementPunishment();
    }

    //Reset agent if next checkpoint isn't reached in 1 Minute
    if (enviormentHandler.GetTimerCheckpoint() > enviormentHandler.resetTimer)
    {
      enviormentHandler.SaveRound(games-1, false);
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
    var action = new float[3];
    if (addOutputs)
    {
      action = new float[3];
      //steering
      action[0] = Input.GetAxis("Horizontal");
      //Accelaration & Brakes
      action[1] = Input.GetAxis("Vertical");
      action[2] = Input.GetAxis("Vertical");
    }
    else
    {
      action = new float[2];
      //steering
      action[0] = Input.GetAxis("Horizontal");
      //Accelaration & Brakes
      action[1] = Input.GetAxis("Vertical");
    }
      return action;
  }

  private void OnCollisionEnter(Collision collision)
  {
    if (collision.gameObject.tag == "wall")
    {
      AddReward(-1.0f);
      Debug.Log("Reward removed (Wall)");
      enviormentHandler.SaveRound(games-1, false);
      Done();
    }
  }
  private void OnTriggerEnter(Collider other)
  {
    if (other.gameObject == nextCheckpoint)
    {
      //updating next checkpoint
      enviormentHandler.CheckCheckpoint(other);
      if (enviormentHandler.IsFinish())
      {
        enviormentHandler.SaveRound(games-1, true);
        Done();
        AddReward(1f);
      }
      else
      {
        AddReward(.3f);
      }
      UpdateCheckpoint();
      Debug.Log("Reward added (Checkpoint)");
    }
  }

  private void UpdateCheckpoint()
  {
    nextCheckpoint = enviormentHandler.NextCheckpoint();
    carController.nextCheckpoint = enviormentHandler.NextCheckpoint();
  }

  private void NoMovementPunishment()
  {
    AddReward(-.001f);
  }

  //functions to keep track in realtime of the reward
  //private void AddMyReward(float amount)
  //{
  //  AddReward(amount);
  //  reward += amount;
  //}
}
