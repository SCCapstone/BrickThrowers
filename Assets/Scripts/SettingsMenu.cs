// Copyright 2025 Brick Throwers
// kjthao 2024
// SettingsMenu.cs - Manages the settings menu for the game.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour {
  public AudioMixer audioMixer;

  public void SetVolume(float volume) {
    //Debug.Log(volume); // checks in console when slider moves!
    audioMixer.SetFloat("volume", volume); // must be named correctly here and in unity (exposed param.)
  }
}
