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

  // Actions
  public static event System.Action<bool> onGodModeActivated;

  void Awake() {
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

  private void ToggleGodModeIndicator(bool buttonStatus) {
    isGodModeActive = buttonStatus;
    if (isGodModeActive) {
      Debug.Log($"Action recieved. God mode activated. Status: {isGodModeActive}");
    } else {
      Debug.Log($"Action recieved. God mode deactivated. Status: {isGodModeActive}");
    }
  }

  private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
    // Get the name of the current active scene.
    activeSceneName = SceneManager.GetActiveScene().name;
    onGodModeActivated?.Invoke(isGodModeActive);
    Debug.Log($"scene loaded you are at {activeSceneName}");
  }
}
