// Copyright 2025 Brick Throwers
// kjthao 2024
// MainMenu.cs - Manages the main menu and game state.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
  public void PlayGame() {
    SceneManager.LoadSceneAsync(1);
  }

  public void QuitGame() {
    Application.Quit();
  }
}
