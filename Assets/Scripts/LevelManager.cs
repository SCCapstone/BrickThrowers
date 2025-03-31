using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
  private static LevelManager Instance;

  private int score = 0;
  private int maxScore = 0;

  ArtifactClass[] artifacts;

   private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

  void Start()
  {
    maxScore = 0;
    CalculateLevelScore();
  }

  void Update()
  {

  }

  public void AddScore(int val)
  {
    score += val;
  }

  public void CalculateLevelScore()
  {
    artifacts = GameObject.FindObjectsOfType<Collectible>();

    foreach (ArtifactClass item in artifacts)
    {
      maxScore += item.GetValue();
      // Debug.Log("Found collectible");
    }
  }
}