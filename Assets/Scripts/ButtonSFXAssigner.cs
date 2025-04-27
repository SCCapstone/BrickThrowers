// Copyright 2025 Brick Throwers
// ButtonSoundAssigner.cs - Assigns sound effects to buttons in the scene.
using UnityEngine;
using UnityEngine.UI;

public class ButtonSoundAssigner : MonoBehaviour {
  private void Start() {
    // Find all buttons in the scene
    Button[] buttons = FindObjectsOfType<Button>();

    // Assign the click sound to each button
    foreach (Button button in buttons) {
      if (SoundManager.Instance != null) {
        SoundManager.Instance.AssignButtonClickSound(button);
      }
    }
  }
}
