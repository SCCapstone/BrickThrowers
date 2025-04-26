using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SummaryScreenUI : MonoBehaviour {

  public GameObject summaryScreen;
  [Header("Summary")]
  [SerializeField] Text Artifacts;

  Player player;
  public void SetSummary(Player player) {

    Artifacts.text = "" + player.artifactsGot;

  }

  public void gameOver(Player player) {
    if (player.oxygenLevel <= 0) {
      summaryScreen.SetActive(true);
      SetSummary(player);
    }
  }

  public void ButtonClicked() {
    Debug.Log("Button Clicked");
  }

  public void ReturntoMainMenu() {

    SceneManager.LoadScene("Main Menu");

  }

}
