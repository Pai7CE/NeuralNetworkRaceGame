using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTimer
{

  public float timerGame { get; set; }
  public float timerRound { get; set; }
  public float timerCheckpoint { get; set; }
  private float resetTime;

  public GameTimer(float resetTime)
  {
    this.resetTime = resetTime;
    timerGame = 0;
    timerCheckpoint = 0;
    timerRound = 0;
  }

  //updates time
  public void UpdateTime()
  {
    timerCheckpoint += Time.deltaTime;
    timerGame += Time.deltaTime;
    timerRound += Time.deltaTime;
  }



}
