// Copyright 2025 Brick Throwers
// kjthao
// LobbyMenu.cs - Manages the lobby menu and countdown.
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.Universal;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyMenu : MonoBehaviour {
  private LobbyCountdown lobbyCountdown;

  private void Start() {
    lobbyCountdown = FindObjectOfType<LobbyCountdown>();
  }
  /// <summary>
  /// Runs the game by starting the countdown.
  /// </summary>
  /// <param name="sceneIndexNum"></param>
  public void RunGame(int sceneIndexNum) {
    lobbyCountdown.StartCountdown(sceneIndexNum);

  }
}
