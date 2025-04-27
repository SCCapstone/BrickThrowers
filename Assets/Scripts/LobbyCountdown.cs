// Copyright 2025 Brick Throwers
// kjthao
// LobbyCountdown.cs - Handles the countdown before starting a game scene.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class LobbyCountdown : MonoBehaviour {
  public GameObject countdownPanel;
  public TMP_Text countdownText;
  public GameObject mapPanel;
  public AudioClip countdownBeep;
  public AudioClip startBeep;
  private AudioSource audioSource;

  private int sceneToLoad;

  private void Start() {
    countdownPanel.SetActive(false);
    audioSource = gameObject.AddComponent<AudioSource>();

  }
  /// <summary>
  /// Starts the countdown for the game scene.
  /// </summary>
  /// <param name="sceneIndex"></param>
  public void StartCountdown(int sceneIndex) {
    sceneToLoad = sceneIndex; // need to store the map first!
    mapPanel.SetActive(false); // then need to hide map panel as the countdown happens:)
    countdownPanel.SetActive(true);
    StartCoroutine(CountdownRoutine());
  }
  /// <summary>
  /// Starts the countdown routine.
  /// </summary>
  /// <returns></returns>
  private IEnumerator CountdownRoutine() {
    int countdown = 3;

    while (countdown > 0) {
      countdownText.text = countdown.ToString();
      audioSource.PlayOneShot(countdownBeep);
      yield return new WaitForSeconds(countdownBeep.length);
      yield return new WaitForSeconds(0.5f);
      countdown--;
    }

    countdownText.text = "Start!";
    audioSource.PlayOneShot(startBeep);
    yield return new WaitForSeconds(startBeep.length);
    yield return new WaitForSeconds(0.5f);

    SceneManager.LoadSceneAsync(sceneToLoad);
  }
}
