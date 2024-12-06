// kjthao
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.Universal;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyMenu : MonoBehaviour
{
    public AudioClip playMusic;
    public AudioClip bgMusic;

    public void RunGame(int sceneIndexNum)
    {
        if (sceneIndexNum == 3 || sceneIndexNum == 2)
            SoundManager.Instance.PlayBackgroundMusic(playMusic);
        else
            SoundManager.Instance.PlayBackgroundMusic(bgMusic);
        SceneManager.LoadSceneAsync(sceneIndexNum); //edited to handle multiple scenes
    }
}
