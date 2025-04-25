// kjthao 2024
// God mode additions by Reshlynt.
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour {
  // Actions
  public static event Action<bool> onGodModeActivated;

  // Game Objects
  [SerializeField]
  GameObject pauseMenu;

  [SerializeField]
  GameObject optionsMenu;

  [SerializeField]
  private Image godModeIndicator;

  // Sound
  public AudioClip bgMusic;

  // Button click
  private bool isButtonClicked = false;

  #region Setup Functions

  #endregion
  #region Pause Menu Functions
  public void Pause() {
    pauseMenu.SetActive(true);
    Time.timeScale = 0;
  }

  public void MainMenu() {
    SceneManager.LoadSceneAsync(0);
    SoundManager.Instance.PlayBackgroundMusic(bgMusic);
  }
  public void Lobby() {
    SceneManager.LoadSceneAsync(1);
    SoundManager.Instance.PlayBackgroundMusic(bgMusic);
  }

  public void Resume() {
    pauseMenu.SetActive(false);
    Time.timeScale = 1;
  }

  public void ShowOptionsMenu() {
    optionsMenu.SetActive(true);
  }

  public void HideOptionsMenu() {
    optionsMenu.SetActive(false);
  }
  #endregion
  /// <summary>
  /// Invokes the GodMode event.
  /// </summary>
  public void GodMode() {
    try {
      // Set the indicator color, godModeIndicator, which is an Image, to the color green when it is clicked.
      // By default, god mode is disabled.
      // If it is clicked again, god mode has been disabled. Thus, return the Image to the color red.

      if (!isButtonClicked) {
        godModeIndicator.color = Color.green;
        isButtonClicked = true;
      } else {
        godModeIndicator.color = Color.red;
        isButtonClicked = false;
      }
      onGodModeActivated?.Invoke(isButtonClicked);

    } catch (Exception e) {
      Debug.Log("Error: " + e.Message);
    }
  }
}
