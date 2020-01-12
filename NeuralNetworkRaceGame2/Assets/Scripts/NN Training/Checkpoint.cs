using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public List<GameObject> checkpoints = new List<GameObject>();

    private int checkpointCount;
    private int finish;

    // Start is called before the first frame update
    void Start()
    {
        checkpointCount = 0; //Starting with 0
    }


    // Update is called once per frame
    void Update()
    {
        
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

    private void resetCheckpoints()
    {
        checkpointCount = 0;
    }
}
