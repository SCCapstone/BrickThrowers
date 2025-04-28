// Copyright 2025 Brick Throwers
// VolumeSettings.cs - Manages the volume settings for the game.
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour {
  [SerializeField] private AudioMixer myMixer;
  [SerializeField] private Slider masterVolume;
  [SerializeField] private Slider musicSlider;

  public void SetMasterVolume() {
    float volume = masterVolume.value;
    myMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
  }

  public void SetMusicVolume() {
    float volume = musicSlider.value;
    myMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
  }
}
