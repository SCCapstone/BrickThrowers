// Copyright 2025 Brick Throwers
// SoundManager.cs - Manages the sound effects and background music for the game.
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour {
  public static SoundManager Instance { get; private set; }

  [SerializeField] private AudioSource audioSource;
  [SerializeField] private AudioSource musicSource;
  [SerializeField] private AudioClip buttonClickSFX;
  [SerializeField] private AudioClip bgMusic;
  [SerializeField] private AudioClip gameMusic;

  private const string MAIN_MENU = "Main Menu";
  private const string LOBBY = "Lobby Scene";
  #region Setup
  private void Awake() {
    if (Instance != null && Instance != this) {
      Destroy(gameObject);
      return;
    }
    Instance = this;
    DontDestroyOnLoad(gameObject);
    PlayBackgroundMusic(bgMusic);
  }
  private void OnEnable() {
    SceneManager.sceneLoaded += OnSceneLoaded;
  }
  private void OnDisable() {
    SceneManager.sceneLoaded -= OnSceneLoaded;
  }
  #endregion

  public void PlayButtonClickSound() {
    if (audioSource != null && buttonClickSFX != null) {
      audioSource.PlayOneShot(buttonClickSFX);
    }
  }
  /// <summary>
  /// Play the background music. If a music clip is provided, it will be played instead of the default one.
  /// </summary>
  /// <param name="musicClip"></param>
  public void PlayBackgroundMusic(AudioClip musicClip = null) {
    if (musicSource == null)
      return;

    if (musicClip != null)
      musicSource.clip = musicClip;

    if (musicSource.clip != null) {
      if (!musicSource.isPlaying || musicSource.clip != musicClip) {
        musicSource.loop = true;
        musicSource.Play();
      }
    }
  }
  /// <summary>
  /// Stop the background music.
  /// </summary>
  public void StopBackgroundMusic() {
    if (musicSource != null) {
      musicSource.Stop();
    }
  }
  /// <summary>
  /// Bu
  /// </summary>
  /// <param name="button"></param>
  public void AssignButtonClickSound(Button button) {
    button.onClick.AddListener(PlayButtonClickSound);
  }
  #region Music Switching Logic
  /// <summary>
  /// Switch music based on the scene loaded.
  /// If lobby and main menu, the load the bgMusic.
  /// Otherwise, load the gameMusic.
  /// </summary>
  /// <param name="scene"></param>
  /// <param name="mode"></param>
  private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, LoadSceneMode mode) {

    if (scene.name == MAIN_MENU || scene.name == LOBBY) {
      SetMenuAndLobbyMusic();
    } else {
      SetGameMusic();
    }
  }

  private void SetGameMusic() {
    PlayBackgroundMusic(gameMusic);
  }
  private void SetMenuAndLobbyMusic() {
    PlayBackgroundMusic(bgMusic);
  }
  #endregion
}
