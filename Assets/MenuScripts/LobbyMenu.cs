// kjthao
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyMenu : MonoBehaviour
{
    public void RunGame()
    {
        SceneManager.LoadSceneAsync(2); // our samplescene scene
    }
}
