using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterOfMass : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody target;
    public Transform newCenterOfMass;

    private Vector3 offset = new Vector3(-16.8f, -1f, -21.9f);
    void Start()
    {
        target.centerOfMass = newCenterOfMass.position + offset;        
    }

}
