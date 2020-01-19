using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private List<GameObject> checkpoints = new List<GameObject>();
    public GameObject test;

    private int checkpointCount;

    // Start is called before the first frame update
    void Start()
    {
        //Init list
        Debug.Log(test.transform.childCount);
        for (int i=0; i < test.transform.childCount; i++)
        {
            checkpoints.Add(test.transform.GetChild(i).gameObject);
        }
        checkpointCount = 0; //Starting with 0        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == checkpoints[checkpointCount])
        {
            Debug.Log("Checkpoint [" + (checkpointCount + 1) + "/" + checkpoints.Count + "] reached" );
            checkpointCount++;
        }
        if(checkpointCount == checkpoints.Count) //if the finish line is reached..
        {
            Debug.Log("Finish!");
            resetCheckpoints();
        }
    }

    public void resetCheckpoints()
    {
        checkpointCount = 0;
    }

    public GameObject nextCheckpoint() //returns the next checkpoint
    {
        return checkpoints[checkpointCount];
    }
}
