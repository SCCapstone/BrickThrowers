// Copyright 2025 Brick Throwers
// SummaryScreenUI.cs - Manages the summary screen UI for the game.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;
using System.Threading.Tasks;


public class SummaryScreenUI : MonoBehaviour {

  public GameObject summaryScreen;

  [Header("Summary")]
  [SerializeField] TextMeshProUGUI artifact;
  [SerializeField] TextMeshProUGUI score;
  [SerializeField] TextMeshProUGUI missed;
  [SerializeField] TextMeshProUGUI exp;
  [SerializeField] TextMeshProUGUI coins;

  [SerializeField] private LevelManager lm;

  Player player;

  // Actions
  public static Action<int> transferCurrency;

  public void SetSummary(Player player) {

    artifact.text = "" + player.artifactsGot;

  }
  /// <summary>
  /// Prepares the summary screen for display.
  /// </summary>
  public void SetSummary() {
    artifact.text = lm.Collected.ToString();
    score.text = lm.Score.ToString();
    missed.text = (lm.Artifacts.Length - lm.Collected).ToString();
    exp.text = lm.CalculateExp().ToString();
    coins.text = "$" + lm.CalculateCurr().ToString();
  }

  public void gameOver(Player player) {
    if (player.oxygenLevel <= 0) {
      summaryScreen.SetActive(true);
      SetSummary();
    }
  }

  public void ButtonClicked() {
    Debug.Log("Button Clicked");
  }

  public void ReturntoMainMenu() {

    SceneManager.LoadScene("Main Menu");

  }

  #region Scene Management
  public async void ReturnToLobby() {
    transferCurrency?.Invoke(lm.Score);
    await Task.Delay(500); // Wait for 1 second
    SceneManager.LoadSceneAsync(1);
  }
  #endregion
}
