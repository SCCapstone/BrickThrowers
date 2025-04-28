// Copyright 2025 Brick Throwers
// FlashlightController.cs - Controls the flashlight's rotation and activation.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightController : MonoBehaviour {
  public Transform player;
  public float rotationSpeed = 100f;
  [SerializeField] private GameObject flashlight;
  private bool isOn = false;

  // Start is called before the first frame update
  void Start() {
    flashlight.SetActive(isOn);
  }
  private void OnEnable() {
    EquipmentClass.onFlashlightUse += TriggerFlashlight;
  }

  private void OnDisable() {
    EquipmentClass.onFlashlightUse -= TriggerFlashlight;
  }


  // I want a flashlight to activate only when the R key is pressed. Already, a use key is present
  // as part of the ItemInteraction script. I will need to a way so that the flashlight is only
  // activated when the R key is pressed, and prefably within the domain of the ItemInteraction script.

  // Update is called once per frame
  void Update() {
    RotateFlashlight();
  }

  void RotateFlashlight() {
    Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

    Vector3 direction = mousePos - player.transform.position;
    direction.z = 0;

    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

    transform.rotation = Quaternion.Euler(0, 0, angle);
  }

  /// <summary>
  /// Activates the flashlight, or deactivates it if it is already on.
  /// </summary>
  private void TriggerFlashlight() {
    if (isOn) {
      flashlight.SetActive(false);
      isOn = false;
    } else {
      flashlight.SetActive(true);
      isOn = true;
    }
  }

}
