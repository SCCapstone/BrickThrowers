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
    public AudioClip bgMusic;

    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
    }

    public void Lobby()
    {
        SceneManager.LoadSceneAsync(1); 
        SoundManager.Instance.PlayBackgroundMusic(bgMusic);
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;

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
