using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set;}
    
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioClip buttonClickSFX;
    [SerializeField] private AudioClip bgMusic;

    void Start()
    {
        
    }

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        PlayBackgroundMusic(bgMusic);
    }

    public void PlayButtonClickSound() {
        if (audioSource != null && buttonClickSFX != null)
            {
                audioSource.PlayOneShot(buttonClickSFX);
            }
    }

    public void PlayBackgroundMusic(AudioClip musicClip = null) {
        // if (musicSource != null && bgMusic != null) {
        //     musicSource.clip = bgMusic;
        //     musicSource.loop = true;
        //     musicSource.Play();
        // }

        if (musicSource == null)
            return;
        
        if (musicClip != null)
            musicSource.clip = musicClip;

            if (musicSource.clip != null) {
                if(!musicSource.isPlaying || musicSource.clip != musicClip) {
                    musicSource.loop = true;
                    musicSource.Play();
                }
            }
    }

    public void StopBackgroundMusic() {
        if(musicSource != null) {
            musicSource.Stop();
        }
    }
}
