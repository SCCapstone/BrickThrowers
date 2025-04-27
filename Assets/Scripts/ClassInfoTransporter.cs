// Copyright 2025 Brick Throwers
// ClassInfoTransporter.cs - Handles the transportation of class information between scenes.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClassInfoTransporter : MonoBehaviour {
  // Info to transport
  private RuntimeAnimatorController selectedClassAnimator;
  private string selectedClass;

  // Actions
  public static event System.Action<RuntimeAnimatorController, string> transportClassInfo;

  #region Setup Functions
  private void Awake() {
    DontDestroyOnLoad(this.gameObject);
  }
  private void OnEnable() {
    ClassSelector.onClassSelected += AcquireClassInfo;
    SceneManager.sceneLoaded += OnSceneLoaded;
  }
  private void OnDisable() {
    ClassSelector.onClassSelected -= AcquireClassInfo;
    SceneManager.sceneLoaded -= OnSceneLoaded;
  }
  #endregion
  /// <summary>
  /// Sets class information to be transported to game scenes.
  /// </summary>
  /// <param name="rtAnimator">Selected class animator.</param>
  /// <param name="className">Selected class name.</param>
  private void AcquireClassInfo(RuntimeAnimatorController rtAnimator, string className) {
    selectedClassAnimator = rtAnimator;
    selectedClass = className;
  }
  /// <summary>
  /// Invoke signal to transport class information to game scenes.
  /// </summary>
  /// <param name="scene"></param>
  /// <param name="mode"></param>
  private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
    transportClassInfo?.Invoke(selectedClassAnimator, selectedClass);
  }
}
