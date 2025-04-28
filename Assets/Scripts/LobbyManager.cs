// Copyright 2025 Brick Throwers
// LobbyManager.cs - Manages the lobby class selection and transitions.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : MonoBehaviour {
  private string selectedClass;

  void Start() {
    selectedClass = "None";  // Default value
  }
  /// <summary>
  /// Selects the Harpooner class.
  /// </summary>
  public void SelectHarpooner() {
    selectedClass = "Harpooner";
    Debug.Log("Harpooner selected");
  }
  /// <summary>
  /// Selects the Diver class.
  /// </summary>
  public void SelectPorter() {
    selectedClass = "Porter";
    Debug.Log("Porter selected");
  }

  // Method to retrieve the selected class when transitioning to gameplay
  public string GetSelectedClass() {
    return selectedClass;
  }
  /// <summary>
  /// Exits the class selection and saves the selected class to PlayerPrefs.
  /// </summary>
  public void ExitClassSelection() {
    // Save the selected class to PlayerPrefs
    PlayerPrefs.SetString("PlayerClass", selectedClass);
    PlayerPrefs.Save();  // Make sure to save the data

    // Optionally, debug log to check that class is saved
    Debug.Log("Class saved: " + selectedClass);

    // Close the class selection panel (you may disable the panel or load a different UI)
    gameObject.SetActive(false); // Example of deactivating the class selection panel
  }

}
