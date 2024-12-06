// kjthao
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyMenu : MonoBehaviour
{
    public void RunGame(int sceneIndexNum)
    {
        SceneManager.LoadSceneAsync(sceneIndexNum); //edited to handle multiple scenes
    }
}
