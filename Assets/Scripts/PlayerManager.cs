// Copyright 2025 Brick Throwers
// Manages the player animations and class selection.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour {
  /* the script thats responsible for spawning the player prefabs into the maps!*/
  public GameObject player;
  public GameObject diverPrefab;
  public GameObject harpoonerPrefab;
  public GameObject porterPrefab;
  public RuntimeAnimatorController diverAnimatorController;
  public RuntimeAnimatorController harpoonerAnimatorController;
  public RuntimeAnimatorController porterAnimatorController;

  private Animator playerAnimator;
  /// <summary>
  ///  Applies the selected class to the player and updates the animator.
  /// </summary>
  /// <param name="className"></param>
  public void ApplyClassAnimation(string className) {
    switch (className) {
      case "Diver":
        playerAnimator.runtimeAnimatorController = diverAnimatorController;
        break;
      case "Harpooner":
        playerAnimator.runtimeAnimatorController = harpoonerAnimatorController;
        break;
      case "Porter":
        playerAnimator.runtimeAnimatorController = porterAnimatorController;
        break;
      default:
        Debug.LogWarning("Unknown class: " + className);
        break;
    }
  }
}
