using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
  public static LevelManager Instance;

  private int score = 0;
  private int maxScore = 0;
  private int collected = 0;

  ArtifactClass[] artifacts;

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
}