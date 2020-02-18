using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReducedDistance : MonoBehaviour
{
    private float oldDist;
    private float newDist;

    private Checkpoint checkpoint;
    private bool distanceReduced;

    // Start is called before the first frame update
    void Start()
    {
        distanceReduced = false;
        checkpoint = gameObject.GetComponent<Checkpoint>();
        //oldDist = Vector3.Distance(gameObject.transform.position, checkpoint.getCheckpoint(0).transform.position); //TODO: 
        oldDist = 10;
    }


    // Update is called once per frame
    private void FixedUpdate()
    {
        distanceReducedToCheckpoint();
    }

    public bool isDistanceReduced()
    {
        return distanceReduced;
    }

    private void distanceReducedToCheckpoint()
    {
        newDist = Vector3.Distance(gameObject.transform.position, checkpoint.nextCheckpoint().transform.position);
        //Debug.Log("newDist " + newDist);
        //Debug.Log("oldDist " + oldDist);
        if (oldDist > newDist)
        {
            oldDist = newDist;
            //Debug.Log(oldDist + " " + newDist);
            //Debug.Log("true");
            distanceReduced = true;
            
        }
        else
        {
            //Debug.Log("false");
            distanceReduced = false;
        }
    }
    
}
