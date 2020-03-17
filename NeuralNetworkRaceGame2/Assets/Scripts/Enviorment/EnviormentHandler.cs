using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnviormentHandler : MonoBehaviour
{

  public float resetTimer;

  //Components
  private GameTimer gameTimer;
  private CheckpointHandler checkpointHandler;

  // Start is called before the first frame update
  void Start()
  {
    gameTimer = new GameTimer(resetTimer);
    checkpointHandler = new CheckpointHandler();
  }

  // Update is called once per frame
  void Update()
  {
    gameTimer.UpdateTime();
  }

  #region GameTimer

  public void ResetTimerCheckpoint()
  {
    gameTimer.timerCheckpoint = 0;
  }

  public float GetTimerCheckpoint()
  {
    return gameTimer.timerCheckpoint;
  }

  public float GetTimerGame()
  {
    return gameTimer.timerGame;
  }
  #endregion

  #region 

  public void ResetCheckpoints()
  {
    checkpointHandler.ResetCheckpoints();
    ResetTimerCheckpoint();
  }

  public GameObject NextCheckpoint()
  {
    return checkpointHandler.NextCheckpoint();
  }

  public void CheckCheckpoint(Collider other)
  {
    if (checkpointHandler.CheckCheckpoint(other))
    {
      ResetTimerCheckpoint();
    }
  }

  #endregion

}
