// Copyright 2025 Brick Throwers
// SoundManager.cs - Manages the sound effects and background music for the game.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour {
  public static SoundManager Instance { get; private set; }

  [SerializeField] private AudioSource audioSource;
  [SerializeField] private AudioSource musicSource;
  [SerializeField] private AudioClip buttonClickSFX;
  [SerializeField] private AudioClip bgMusic;

  void Start() {

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
}
