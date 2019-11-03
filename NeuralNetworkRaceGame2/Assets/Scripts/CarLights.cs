using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarLights : MonoBehaviour
{
    // Start is called before the first frame update

    public Light redLight;
    public Light blueLight;

    public LensFlare redFlare;
    public LensFlare blueFlare;

    public float maxInstensityLights;
    public float maxIntensityFlares;
    public float speed;

    private float trueMaxIntensity;
    private bool resume = false;

    void Start()
    {
        trueMaxIntensity = maxInstensityLights / 2;
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
            redLight.intensity = trueMaxIntensity * Mathf.Cos(Time.time * speed) + trueMaxIntensity;
            blueLight.intensity = -trueMaxIntensity * Mathf.Cos(Time.time * speed) + trueMaxIntensity;

            redFlare.brightness = -maxIntensityFlares * Mathf.Cos(Time.time * speed) + maxIntensityFlares;
            blueFlare.brightness = maxIntensityFlares * Mathf.Cos(Time.time * speed) + maxIntensityFlares;
        }
        else
        {
            redLight.intensity = 0;
            blueLight.intensity = 0;
            
            redFlare.brightness = 0;
            blueFlare.brightness = 0;
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
