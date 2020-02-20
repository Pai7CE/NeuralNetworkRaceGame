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
        updateTime();
    }
    //updates time
    private void updateTime()
    {
        timer += Time.deltaTime;
    }
    //resets time
    public void resetTimer()
    {
        timer = 0;
    }
    //returns timer
    public float getTimer()
    {
        return timer;
    }
}
