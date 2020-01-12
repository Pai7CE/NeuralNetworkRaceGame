using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterOfMass : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody target;
    public Transform newCenterOfMass;

    private Vector3 offset = new Vector3(-19.8f, -0.1f, -5.2f); //no idea where the offset is from
    void Start()
    {
        target.centerOfMass = newCenterOfMass.position - target.position;        
    }

}
