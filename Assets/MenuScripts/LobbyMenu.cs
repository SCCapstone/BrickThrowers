// kjthao
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyMenu : MonoBehaviour
{
    [SerializeField] private AudioClip gameBgMusic;
    [SerializeField] private AudioClip lobbyBgMusic;

    public void RunGame()
    {
        SceneManager.LoadSceneAsync(2); // our samplescene scene
        SoundManager.Instance.StopBackgroundMusic();
        SoundManager.Instance.PlayBackgroundMusic(gameBgMusic);
    }
}
