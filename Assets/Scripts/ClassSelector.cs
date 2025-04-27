// Copyright 2025 Brick Throwers
// // ClassSelector.cs - Handles the selection of player classes and updates the player's animator and class scripts.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ClassSelector : MonoBehaviour {
  public GameObject player;
  public RuntimeAnimatorController diverAnimatorController;
  public RuntimeAnimatorController harpoonerAnimatorController;
  public RuntimeAnimatorController porterAnimatorController;

  private Animator playerAnimator;

  public Button diverButton;
  public Button harpoonerButton;
  public Button porterButton;

  public TextMeshProUGUI classIndicatorText;
  private string selectedClass;

  // New fields for audio
  public AudioClip classSelectionSound;  // Drag your sound clip here in the Inspector
  private AudioSource audioSource;

  // Action
  public static event Action<RuntimeAnimatorController, string> onClassSelected;

  void Start() {
    playerAnimator = player.transform.GetChild(3).GetComponent<Animator>();

    // Initialize the AudioSource
    audioSource = GetComponent<AudioSource>();
    if (audioSource == null) {
      audioSource = gameObject.AddComponent<AudioSource>();
    }

    diverButton.onClick.AddListener(SelectDiver);
    harpoonerButton.onClick.AddListener(SelectHarpooner);
    porterButton.onClick.AddListener(SelectPorter);
  }
  /// <summary>
  /// Disables all class scripts on the player.
  /// </summary>
  private void DisableAllClassScripts() {
    //var diver = player.GetComponent<Diver>();
    //if (diver != null) diver.enabled = false;

    var harpooner = player.GetComponent<Harpooner>();
    if (harpooner != null) harpooner.enabled = false;

    var porter = player.GetComponent<Porter>();
    if (porter != null) porter.enabled = false;
  }
  /// <summary>
  /// Assigns the selected class to the player and updates the animator.
  /// </summary>
  public void SelectDiver() {
    DisableAllClassScripts();

    playerAnimator.runtimeAnimatorController = diverAnimatorController;
    selectedClass = "Diver";
    ClassSelectionData.SelectedClass = selectedClass;

    var diver = player.GetComponent<Diver>();
    if (diver != null) diver.enabled = true;

    UpdateClassIndicator();
    onClassSelected?.Invoke(diverAnimatorController, selectedClass);
  }
  /// <summary>
  /// Assigns the selected class to the player and updates the animator.
  /// </summary>
  public void SelectHarpooner() {
    DisableAllClassScripts();

    playerAnimator.runtimeAnimatorController = harpoonerAnimatorController;
    selectedClass = "Harpooner";
    ClassSelectionData.SelectedClass = selectedClass;

    var harpooner = player.GetComponent<Harpooner>();
    if (harpooner != null) harpooner.enabled = true;

    UpdateClassIndicator();
    onClassSelected?.Invoke(harpoonerAnimatorController, selectedClass);
  }
  /// <summary>
  /// Assigns the selected class to the player and updates the animator.
  /// </summary>
  public void SelectPorter() {
    DisableAllClassScripts();

    playerAnimator.runtimeAnimatorController = porterAnimatorController;
    selectedClass = "Porter";
    ClassSelectionData.SelectedClass = selectedClass;

    var porter = player.GetComponent<Porter>();
    if (porter != null) porter.enabled = true;

    UpdateClassIndicator();
    onClassSelected?.Invoke(porterAnimatorController, selectedClass);
  }
  /// <summary>
  /// Updates the class indicator text and plays the sound.
  /// </summary>
  private void UpdateClassIndicator() {
    classIndicatorText.text = $"You have chosen {selectedClass}!";
    classIndicatorText.color = Color.green;

    // Play the sound when the class is updated
    if (audioSource != null && classSelectionSound != null) {
      audioSource.PlayOneShot(classSelectionSound);
    }
  }
}
