// Copyright 2025 Brick Throwers
// // LevelManager.cs - Manages the level state and score.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {
  public static LevelManager Instance;

  [SerializeField] private int score = 0;
  public int Score {
    get { return score; }
    set { score = value; }
  }

  private int maxScore = 0;
  public int MaxScore {
    get { return maxScore; }
    set { maxScore = value; }
  }

  private int collected = 0;
  public int Collected {
    get { return collected; }
    set { collected = value; }
  }

  private ArtifactClass[] artifacts;
  public ArtifactClass[] Artifacts {
    get { return artifacts; }
    set { artifacts = value; }
  }

  //private void Awake() {
  //  if (Instance == null)

  //}

  void Start() {
    maxScore = 0;
    collected = 0;
    CalculateLevelScore();
  }

  void Update() {

  }
  /// <summary>
  /// Adds score to the player. Adds 1 to the collected count of artifacts.
  /// </summary>
  /// <param name="val"></param>
  public void AddScore(int val) {
    score += val;
    collected++;
  }
  /// <summary>
  /// Calculates the level score based on the artifacts in the level.
  /// </summary>
  public void CalculateLevelScore() {
    int artifactsLength = 0;
    GameItem[] items = GameObject.FindObjectsOfType<GameItem>();
    List<ArtifactClass> itemsToCovert = new List<ArtifactClass>();

    foreach (GameItem item in items) {
      if (item.gameItemClass.itemType == ItemClass.ItemType.artifact) {
        artifactsLength++;
        itemsToCovert.Add((ArtifactClass)item.gameItemClass);
      }
    }

    artifacts = itemsToCovert.ToArray();

    foreach (ArtifactClass item in artifacts) {
      maxScore += item.value;
      Debug.Log("Found collectible");
    }
  }

  public int CalculateExp() {
    return score * 10;  // Change when game testing, needs balancing
  }

  public int CalculateCurr() {
    return score / 10; // Change when game testing, needs balancing
  }

  public void UpdatePlayer() {
    // Update the player w/ new info
  }
}
