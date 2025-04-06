using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
  public static LevelManager Instance;

  private int score = 0 ;
  public int Score
    {
        get { return score; }
        set { score = value; }
    }
  
  private int maxScore = 0;
  public int MaxScore
    {
        get { return maxScore; }
        set { maxScore = value; }
    }
  
  private int collected = 0;
  public int Collected
    {
        get { return collected; }
        set { collected = value; }
    }

  private ArtifactClass[] artifacts;
  public ArtifactClass[] Artifacts
    {
        get { return artifacts; }
        set { artifacts = value; }
    }

   private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

  void Start()
  {
    maxScore = 0;
    collected = 0;
    CalculateLevelScore();
  }

  void Update()
  {

  }

  public void AddScore(int val)
  {
    score += val;
    collected++;
  }

  public void CalculateLevelScore()
  {
    artifacts = GameObject.FindObjectsOfType<ArtifactClass>();

    foreach (ArtifactClass item in artifacts)
    {
      maxScore += item.GetValue();
      // Debug.Log("Found collectible");
    }
  }

  public int CalculateExp()
  {
    return score * 10;  // Change when game testing, needs balancing
  }

  public int CalculateCurr()
  {
    return score / 10; // Change when game testing, needs balancing
  }

  public void UpdatePlayer()
  {
    // Update the player w/ new info
  }
}