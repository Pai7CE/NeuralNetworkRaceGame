using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarLights : MonoBehaviour
{
    // Start is called before the first frame update

    public Light red;
    public Light blue;

    public float maxInstensity;
    public float speed;

    private float trueMaxIntensity;
    private bool resume = false;

    void Start()
    {
        trueMaxIntensity = maxInstensity / 2;
    }

    // Update is called once per frame
    void Update()
    {
        checkToContinue();
        siren();
    }

    public void siren()
    {
        if (resume)
        {
        Debug.Log(Time.time);
        red.intensity = trueMaxIntensity * Mathf.Cos(Time.time * speed) + trueMaxIntensity;
        blue.intensity = -trueMaxIntensity * Mathf.Cos(Time.time * speed) + trueMaxIntensity;
        }
        else
        {
            red.intensity = 0;
            blue.intensity = 0;
        }
    }
    public void checkToContinue()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            resume = !resume;
        }
    }
}
