// Copyright 2025 Brick Throwers
// GamePauseMenuLobbyLoader.cs - Loads the lobby scene when the player chooses to go back to the lobby from the pause menu.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GamePauseMenuLobbyLoader : MonoBehaviour {
  /*USED in the GamePauseMenu Prefab*/
  public void GoBackToLobby() {
    SceneManager.LoadSceneAsync(1);
  }
}
