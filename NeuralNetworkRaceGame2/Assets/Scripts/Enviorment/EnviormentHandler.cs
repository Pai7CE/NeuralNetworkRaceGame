using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnviormentHandler : MonoBehaviour
{

  public float resetTimer;

  [Header("Evaluation")]
  public bool evaluate;
  public string fileName;
  public int maxGames;
  public int amntTopRounds;

  //Components
  private GameTimer gameTimer;
  private CheckpointHandler checkpointHandler;
  private EvaluationHandler evaluationHandler;

  // Start is called before the first frame update
  void Start()
  {
    gameTimer = new GameTimer(resetTimer);
    checkpointHandler = new CheckpointHandler();
    if (evaluate)
    {
      evaluationHandler = new EvaluationHandler(maxGames, fileName, amntTopRounds);
    }
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

  public void ResetTimerRound()
  {
    gameTimer.timerRound = 0;
  }

  public void SetTimerRound(float timer)
  {
    gameTimer.timerRound = timer;
  }

  public float GetTimerRound()
  {
    return gameTimer.timerRound;
  }
  #endregion

  #region 

  public void ResetCheckpoints()
  {
    checkpointHandler.ResetCheckpoints();
    ResetTimerCheckpoint();
    ResetTimerRound();
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

  public bool IsFinish()
  {
    return checkpointHandler.IsFinish();
  }

  #endregion

  #region Evaluation

  public void SaveRound(int gameNo, bool save)
  {
    if (evaluate)
    {
      if (!save)
      {
        gameTimer.timerRound = 3540f;
      }
      if(evaluate && (gameNo <= maxGames-1))
      {
        evaluationHandler.SaveRound(gameNo, gameTimer.timerRound);
      }
      if(gameNo == maxGames-1)
      {
        evaluationHandler.SaveEvaluation();
        Debug.DebugBreak();
      }
    }
  }

  public void StartEvaluation()
  {
    evaluationHandler.SaveEvaluation();
  }

  #endregion

}
