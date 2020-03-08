using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointHandler : MonoBehaviour
{
  private List<GameObject> checkpoints;
  private GameObject allCheckpoints;

  private TimeBetweenCheckpoints timeCheckpoints;
  private int checkpointCount;

  // Start is called before the first frame update
  void Start()
  {
    timeCheckpoints = gameObject.GetComponent<TimeBetweenCheckpoints>();
    allCheckpoints = GameObject.FindGameObjectWithTag("checkpoints");
    checkpoints = new List<GameObject>();
    //Init list
    for (int i = 0; i < allCheckpoints.transform.childCount; i++)
    {
      checkpoints.Add(allCheckpoints.transform.GetChild(i).gameObject);
    }
    checkpointCount = 0; //Starting with 0        
  }

  public GameObject CheckCheckpoint(Collider other)
  {
    if (other.gameObject == checkpoints[checkpointCount])
    {
      Debug.Log("Checkpoint [" + (checkpointCount + 1) + "/" + checkpoints.Count + "] reached");
      checkpointCount++;
      timeCheckpoints.ResetTimer();
    }
    if (checkpointCount == checkpoints.Count) //if the finish line is reached..
    {
      Debug.Log("Finish!");
      ResetCheckpoints();
    }
    return checkpoints[checkpointCount];
  }

  public void ResetCheckpoints() //resetting checkpoint count
  {
    checkpointCount = 0;
  }

  public GameObject NextCheckpoint() //returns the current next checkpoint
  {
    return checkpoints[checkpointCount];
  }

  public GameObject GetCheckpoint(int i) //returns the next checkpoint
  {
    return checkpoints[i];
  }
}
