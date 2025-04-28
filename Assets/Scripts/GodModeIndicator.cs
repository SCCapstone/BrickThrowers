// Copyright 2025 Brick Throwers
// GodModeIndicator.cs - Manages the God Mode indicator in the game.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GodModeIndicator : MonoBehaviour {
  // Game objects
  [SerializeField]
  private GameObject godModeIndicatorPrefab;

  // Activation
  [SerializeField]
  private bool isGodModeActive = false;

  // Scene names
  [SerializeField] private string lobbySceneName;
  [SerializeField] private string activeSceneName;

  // Singleton
  public static GodModeIndicator Instance { get; private set; }

  // Actions
  public static event System.Action<bool> onGodModeActivated;
  #region Setup Functions
  void Awake() {
    if(Instance != null && Instance != this) {
      Destroy(gameObject);
      return;
    }
    Instance = this;
    lobbySceneName = SceneManager.GetActiveScene().name;
    DontDestroyOnLoad(this.gameObject);
  }

  private void OnEnable() {
    PauseMenu.onGodModeActivated += ToggleGodModeIndicator;
    SceneManager.sceneLoaded += OnSceneLoaded;
  }

  private void OnDisable() {
    PauseMenu.onGodModeActivated -= ToggleGodModeIndicator;
    SceneManager.sceneLoaded -= OnSceneLoaded;
  }
  #endregion
  /// <summary>
  /// Sets the God Mode indicator to be active or inactive.
  /// </summary>
  /// <param name="buttonStatus"></param>
  private void ToggleGodModeIndicator(bool buttonStatus) {
    isGodModeActive = buttonStatus;
    if (isGodModeActive) {
      Debug.Log($"Action recieved. God mode activated. Status: {isGodModeActive}");
    } else {
      Debug.Log($"Action recieved. God mode deactivated. Status: {isGodModeActive}");
    }
  }
  /// <summary>
  /// Invokes signal to update the God Mode indicator when a scene is loaded. Only Player.cs will
  /// have the listener for the signal.
  /// </summary>
  /// <param name="scene"></param>
  /// <param name="mode"></param>
  private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
    // Get the name of the current active scene.
    activeSceneName = SceneManager.GetActiveScene().name;
    onGodModeActivated?.Invoke(isGodModeActive);
    Debug.Log($"scene loaded you are at {activeSceneName}");
  }
}
