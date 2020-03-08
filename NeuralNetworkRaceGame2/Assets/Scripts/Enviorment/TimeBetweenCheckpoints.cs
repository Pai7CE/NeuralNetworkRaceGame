using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeBetweenCheckpoints : MonoBehaviour
{

    private float timer;
    public float resetTime;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTime();
    }
    //updates time
    private void UpdateTime()
    {
        timer += Time.deltaTime;
    }
    //resets time
    public void ResetTimer()
    {
        timer = 0;
    }
    //returns timer
    public float GetTimer()
    {
        return timer;
    }
}
