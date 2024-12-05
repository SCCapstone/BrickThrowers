// kjthao 2024
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour

/* 
Note: Working on trying to figure out the little warning: 
"The referenced script on this Behaviour (Game Object 'Pause Menu') is missing!"
Solved! (had to go inside prefab that I created and asign the script there i think.)
*/

{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject optionsMenu;
    [SerializeField] AudioClip lobbyBgMusic;

    public void Pause()
    {
        pauseMenu.SetActive(true);
    }

    public void Lobby()
    {
        //SceneManager.LoadScene(1); // can use scenename or the scene number
        SoundManager.Instance.PlayBackgroundMusic(lobbyBgMusic);
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);

    }

    public void ShowOptionsMenu()
    {
        optionsMenu.SetActive(true);

    }

    public void HideOptionsMenu()
    {
        optionsMenu.SetActive(false);

    }
}
