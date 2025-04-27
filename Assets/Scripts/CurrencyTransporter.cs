using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * Copyright 2025 Brick Throwers CSCE-492
 * Transport money from the game scenes into the lobby scene and add to the current currency.
 */
public class CurrencyTransporter : MonoBehaviour {
  // Money to transport
  private static int moneyFromGame;

  // Scene names
  private const string LOBBY_NAME = "Lobby Scene";
  private string activeScene;

  // Action
  public static event Action<int> transportMoneyNow;

  void Awake() {
    activeScene = SceneManager.GetActiveScene().name;
    DontDestroyOnLoad(this.gameObject);
  }

  private void OnEnable() {
    SummaryScreenUI.transferCurrency += AssignCurrency;
    SceneManager.sceneLoaded += OnSceneLoaded;
  }
  private void OnDisable() {
    SummaryScreenUI.transferCurrency -= AssignCurrency;
    SceneManager.sceneLoaded -= OnSceneLoaded;
  }
  private void AssignCurrency(int value) {
    moneyFromGame = value;
    Debug.Log($"Money assigned: {moneyFromGame}");
  }

  private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
    if (scene.name == LOBBY_NAME) {
      transportMoneyNow?.Invoke(moneyFromGame);
    }
    moneyFromGame = 0;
  }
}
