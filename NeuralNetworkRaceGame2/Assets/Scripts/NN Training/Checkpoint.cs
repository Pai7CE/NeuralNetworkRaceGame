using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private List<GameObject> checkpoints = new List<GameObject>();
    public GameObject test;

    private TimeBetweenCheckpoints timeCheckpoints;
    private int checkpointCount;

    // Start is called before the first frame update
    void Start()
    {
        timeCheckpoints = gameObject.GetComponent<TimeBetweenCheckpoints>();
        //Init list
        for (int i=0; i < test.transform.childCount; i++)
        {
            checkpoints.Add(test.transform.GetChild(i).gameObject);
        }
        checkpointCount = 0; //Starting with 0        
    }

    public GameObject checkCheckpoint(Collider other)
    {
        if (other.gameObject == checkpoints[checkpointCount])
        {
            Debug.Log("Checkpoint [" + (checkpointCount + 1) + "/" + checkpoints.Count + "] reached"); 
            checkpointCount++;
            timeCheckpoints.resetTimer();
        }
        if (checkpointCount == checkpoints.Count) //if the finish line is reached..
        {
            Debug.Log("Finish!");
            resetCheckpoints();
        }
        return checkpoints[checkpointCount];
    }

    public void resetCheckpoints() //resetting checkpoint count
    {
        checkpointCount = 0;
    }

    public GameObject nextCheckpoint() //returns the current next checkpoint
    {
        return checkpoints[checkpointCount];
    }

    public GameObject getCheckpoint(int i) //returns the next checkpoint
    {
        return checkpoints[i];
    }
}
