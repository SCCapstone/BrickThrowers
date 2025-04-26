// kjthao
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Rendering.Universal;
using UnityEngine.SceneManagement;

public class BackToMainMenu : MonoBehaviour {
  public void GoToMainMenu() {
    SceneManager.LoadSceneAsync(0); //this is mainly for the pause screen since the lobbymenu.cs uses the counter
  }
}
