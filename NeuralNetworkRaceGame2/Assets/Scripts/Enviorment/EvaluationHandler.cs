using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class EvaluationHandler
{
  private int maxGames; //amount of games to be played until evaluation is stopped
  private string fileName;
  private string path;
  private float[] roundTimes;
  public int amntTopRounds;

  public EvaluationHandler(int maxGames, string fileName, int amntTopRounds)
  {
    this.maxGames = maxGames;
    this.fileName = fileName;
    roundTimes = new float[maxGames];
    this.amntTopRounds = amntTopRounds;
  }

  private string FormattedTime(float ms)
  {
    //int hours = Mathf.FloorToInt(ms / 3600f);
    int minutes = Mathf.FloorToInt((ms % 3600) / 60);
    int seconds = Mathf.FloorToInt(ms % 60);
    int milliseconds = Mathf.FloorToInt((ms % 1) * 100);
    string formattedTime = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);
    return formattedTime;
  }

  public void SaveEvaluation()
  {
    path = Path.Combine(Application.dataPath, "Evaluation/" + fileName + ".txt");
    DeleteFile();
    string fileContent = "";
    fileContent += GetAllTimes();
    fileContent += "\n";
    fileContent += SuccessfulRounds();
    fileContent += "\n";
    fileContent += GetAverageSuccessfulRounds();
    fileContent += "\n";
    fileContent += GetTopRounds();
    File.WriteAllText(path, fileContent);
  }

  private string GetAverageSuccessfulRounds()
  {
    float avgTime = 0;
    string stringReturn = "";

    List<float> successfulRounds = new List<float>();
    for (int i = 0; i < roundTimes.Length; i++)
    {
      if(roundTimes[i] < 3540f)
      {
        successfulRounds.Add(roundTimes[i]);
      }
    }

    int f = 0;

    foreach(float round in successfulRounds)
    {
      f++;
      avgTime += round;
      stringReturn += f + ".: " + FormattedTime(round) + "\n";
    }

    return "Durchschnitt erfolgreicher Runden: " + FormattedTime((avgTime / successfulRounds.Count)) + "\n" + stringReturn;
  }

  public void SaveRound(int roundNo, float roundTime)
  {
    Debug.Log("Saved " + roundNo + " " + roundTime);
    roundTimes[roundNo] = roundTime;
  }

  public void DeleteFile()
  {
    if (File.Exists(path))
    {
      File.Delete(path);
      File.Delete(path + ".meta");
    }
  }

  private string GetTopRounds()
  {
    string topTimes = "";
    float avgTopTimes = 0;
    
    System.Array.Sort(roundTimes);
    int countedRounds = 0;
    for(int i=0; i < amntTopRounds; i++)
    {
      if(roundTimes[i] < 3540f)
      {
        countedRounds++;
        topTimes += i+1 + ".: " + FormattedTime(roundTimes[i]) + "\n";
        avgTopTimes += roundTimes[i];
      }
    }

    return "Durchschnitt der besten " + countedRounds + " Runden: " + FormattedTime((avgTopTimes / countedRounds)) + "\n" + topTimes;
    
  }

  private string SuccessfulRounds()
  {
    int succesful = 0;
    for (int i = 0; i < roundTimes.Length; i++)
    {
      if(roundTimes[i] < 3540f)
      {
        succesful += 1;
      }
    }
    return "Erfolgreiche Runden: " + succesful + "/" + roundTimes.Length +  "\n";
  }

  private string GetAllTimes()
  {
    string allTimes = "Alle aufgezeichneten Runden: (" + roundTimes.Length + ") \n";

    for (int i = 0; i < roundTimes.Length; i++)
    {
      if (roundTimes[i] < 3540f)
      {
        allTimes += i+1 + ".: " + FormattedTime(roundTimes[i]) + "\n";
      }
      else
      {
        allTimes += i + 1 + ".: " + "N.A. \n";
      }
    }
    return allTimes;
  }
}
