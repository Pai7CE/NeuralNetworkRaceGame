using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turningSiren : MonoBehaviour
{
    public float speed;
    public Light red;
    public Light blue;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        red.transform.Rotate(Vector3.up * speed * Time.deltaTime, Space.World);
        blue.transform.Rotate(Vector3.up * speed * Time.deltaTime, Space.World);
    }
}
