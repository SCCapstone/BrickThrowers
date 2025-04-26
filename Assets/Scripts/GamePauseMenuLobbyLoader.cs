using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GamePauseMenuLobbyLoader : MonoBehaviour {
  /*USED in the GamePauseMenu Prefab*/
  public void GoBackToLobby() {
    SceneManager.LoadSceneAsync(1);
  }
}
