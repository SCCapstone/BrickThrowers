// Copyright 2025 Brick Throwers
// ExperienceManager.cs - Manages the experience points in the game.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceManager : MonoBehaviour {
  public static ExperienceManager Instance;

  public delegate void ExperienceChangeHandler(int amount);
  public event ExperienceChangeHandler onExperienceChange;


  //Singleton Check
  private void Awake() {
    if (Instance != null && Instance != this) {
      Destroy(this);
    } else {
      Instance = this;
    }
  }

  //Adding Experience, Question Mark avoids nulls
  public void AddExperience(int amount) {
    onExperienceChange?.Invoke(amount);
  }
}
