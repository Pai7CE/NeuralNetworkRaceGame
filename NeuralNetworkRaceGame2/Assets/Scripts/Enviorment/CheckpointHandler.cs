using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointHandler
{
  private List<GameObject> checkpoints;
  private GameObject allCheckpoints;

  private int checkpointCount;

  // Start is called before the first frame update
  //void Start()
  //{
  //  allCheckpoints = GameObject.FindGameObjectWithTag("checkpoints");
  //  checkpoints = new List<GameObject>();
  //  //Init list
  //  for (int i = 0; i < allCheckpoints.transform.childCount; i++)
  //  {
  //    checkpoints.Add(allCheckpoints.transform.GetChild(i).gameObject);
  //  }
  //  checkpointCount = 0; //Starting with 0        
  //}

  public CheckpointHandler()
  {
    allCheckpoints = GameObject.FindGameObjectWithTag("checkpoints");
    checkpoints = new List<GameObject>();
    //Init list
    for (int i = 0; i < allCheckpoints.transform.childCount; i++)
    {
      checkpoints.Add(allCheckpoints.transform.GetChild(i).gameObject);
    }
    checkpointCount = 0; //Starting with 0  
  }
  //returns true if the correct Checkpoint is hit
  public bool CheckCheckpoint(Collider other)
  {
    if (other.gameObject == checkpoints[checkpointCount])
    {
      Debug.Log("Checkpoint [" + (checkpointCount + 1) + "/" + checkpoints.Count + "] reached");
      checkpointCount++;
      return true;
    }
    return false;
  }

  public void ResetCheckpoints() //resetting checkpoint count
  {
    checkpointCount = 0;
  }

  public bool IsFinish()
  {
    if(checkpointCount == checkpoints.Count)
    {
      ResetCheckpoints();
      Debug.Log("Finish!");
      return true;
    }
    return false;
  }

  public GameObject NextCheckpoint() //returns the current next checkpoint
  {
    return checkpoints[checkpointCount];
  }

  //public GameObject GetCheckpoint(int i) //returns the next checkpoint
  //{
  //  return checkpoints[i];
  //}
}
