using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClassInfoTransporter : MonoBehaviour
{
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
  private void AcquireClassInfo(RuntimeAnimatorController rtAnimator, string className) {
    selectedClassAnimator = rtAnimator;
    selectedClass = className;
  }
  private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
    transportClassInfo?.Invoke(selectedClassAnimator, selectedClass);
  }
}
