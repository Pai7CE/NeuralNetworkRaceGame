using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReducedDistance : MonoBehaviour
{
    private float oldDist;
    private float newDist;

    private Checkpoint checkpoint;

    // Start is called before the first frame update
    void Start()
    {
        checkpoint = gameObject.GetComponent<Checkpoint>();
        oldDist = Vector3.Distance(gameObject.transform.position, checkpoint.nextCheckpoint().transform.position);
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        distanceReducedToCheckpoint();
        updateCheckpoint();
    }

    public bool distanceReducedToCheckpoint()
    {
        newDist = Vector3.Distance(gameObject.transform.position, checkpoint.nextCheckpoint().transform.position);

        if (oldDist > newDist)
        {
            oldDist = newDist;
            //Debug.Log(oldDist + " " + newDist);
            //Debug.Log("true");
            return true;
            
        }
        else
        {
            //Debug.Log("false");
            return false;
        }
    }
    private void updateCheckpoint()
    {

    }
}
