// Copyright 2025 Brick Throwers
// GameManager.cs - Manages the game state and player interactions.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
  [SerializeField]
  public Player player;
  public GameObject DefeatPanel;


  private void OnEnable() {
    Player.onDeath += Defeat;
  }

  private void OnDisable() {
    Player.onDeath -= Defeat;
  }

  /// <summary>
  /// Should the player lost all their oxygen, the game is over. Show the game summary screen.
  /// </summary>
  public void Defeat() {
    if (player.oxygenLevel <= 0) {
      DefeatPanel.SetActive(true);
      Debug.Log("Game Over");
    }
  }
}
