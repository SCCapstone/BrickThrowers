// Copyright 2025 Brick Throwers
// // EscapeKeyManager.cs - Manages the escape key input and panel visibility in the game.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EscapeKeyManager : MonoBehaviour {
  private PlayerInputActions controls;
  private InputAction escape;
  [SerializeField] private GameObject[] panels;

  // Pause menu specific
  [SerializeField] private GameObject pauseMenuPanel;
  private bool isPaused = false;


  #region Setup Functions
  private void Awake() {
    controls = new PlayerInputActions();
  }

  private void OnEnable() {
    escape = controls.UI.Escape;
    escape.Enable();
    escape.performed += OnCancel;
  }

  private void OnDisable() {
    escape.Disable();
    escape.performed -= OnCancel;
  }

  private void OnCancel(InputAction.CallbackContext contex) {
    // The case where there is no panel open.
    bool anyPanelNotPauseOpen = false;
    for (int i = panels.Length - 1; i >= 0; i--) {
      if (panels[i] != null && panels[i].activeSelf) {
        anyPanelNotPauseOpen = true;
        break;
      }
    }


    if (pauseMenuPanel != null && !pauseMenuPanel.activeSelf && !anyPanelNotPauseOpen) {
      pauseMenuPanel.SetActive(true);
      return;
    }


    for (int i = panels.Length - 1; i >= 0; i--) {
      if (panels[i] != null && panels[i].activeSelf) {
        panels[i].SetActive(false);
        return;
      }
    }
  }
  #endregion
}
