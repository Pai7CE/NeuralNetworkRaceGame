using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LapCompleted : MonoBehaviour
{

    public BoxCollider halfway;
    public BoxCollider finish;

    public GameObject bestMinuteBox;
    public GameObject bestSecondBox;
    public GameObject bestMilliBox;

    private float countMilli;
    private int countSecond;
    private int countMinute;


    private bool halfwayReached;
    private bool initialLap;
    void Start()
    {
        halfwayReached = false;
        initialLap = true;
    }
    //TODO: delete after game is complete
    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.K))
        {
            halfwayReached = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other == halfway)
        {
            halfwayReached = true;
        }
        if(other == finish && halfwayReached == true)
        {
            lapCompleted();
        }
    }

    private void lapCompleted()
    {
        Debug.Log("lap complete");

        if (initialLap)
        {
            updateTimer();
        }
        else
        {
            if(LapTimeManager.MinuteCount <= countMinute)
            {
                if(LapTimeManager.MinuteCount < countMinute)
                {
                    updateTimer();
                }
                else
                {
                    if(LapTimeManager.SecondCount <= countSecond)
                    {
                        if(LapTimeManager.SecondCount < countSecond)
                        {
                            updateTimer();
                        }
                        else
                        {
                            if(LapTimeManager.MilliCount < countMilli)
                            {
                                updateTimer();
                            }
                        }
                    }
                }
            }
        }
              
        LapTimeManager.MilliCount = 0;
        LapTimeManager.SecondCount = 0;
        LapTimeManager.MinuteCount = 0;

        initialLap = false;
        halfwayReached = false;
    }
    private void updateTimer()
    {
        if (LapTimeManager.MilliCount <= 9)
        {
            bestMilliBox.GetComponent<Text>().text = "0" + LapTimeManager.MilliCount;
        }
        else
        {
            bestMilliBox.GetComponent<Text>().text = LapTimeManager.MilliCount + "";
        }
        if (LapTimeManager.SecondCount <= 9)
        {
            bestSecondBox.GetComponent<Text>().text = "0" + LapTimeManager.SecondCount + ".";
        }
        else
        {
            bestSecondBox.GetComponent<Text>().text = LapTimeManager.SecondCount + ".";
        }
        if (LapTimeManager.MinuteCount <= 9)
        {
            bestMinuteBox.GetComponent<Text>().text = "0" + LapTimeManager.MinuteCount + ":";
        }
        else
        {
            bestMinuteBox.GetComponent<Text>().text = LapTimeManager.MinuteCount + ":";
        }
        countMilli = LapTimeManager.MilliCount;
        countSecond = LapTimeManager.SecondCount;
        countMinute = LapTimeManager.MinuteCount;
    }
}
