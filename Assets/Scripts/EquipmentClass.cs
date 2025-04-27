/*
 * Copyright 2025 Scott Do
 * 2/15/2025
 * Handles the equipment items in the game.
 */
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Equipment", menuName = "Items/Equipment")]
public class EquipmentClass : ItemClass {
  public enum EquipType {
    flashlight,
  }

  public static event Action onFlashlightUse; // Action for when the flashlight is used.

  [Header("Equipment")]
  private EquipType EquipmentType;

  public override ItemClass GetItem() {
    return this;
  }

  public EquipmentClass GetTool() {
    return this;
  }

  public override bool Use(Player player) {
    if (onFlashlightUse != null && EquipmentType == EquipType.flashlight) {
      UseFlashlight();
      return true;
    }

    return false;
  }

  /// <summary>
  /// Turns on the flashlight for the player.
  /// </summary>
  /// <returns> True if the flashlight is turned on, false if the action is impeded. </returns>
  private bool UseFlashlight() {
    try {
      onFlashlightUse?.Invoke();
      return true;
    } catch (Exception e) {
      Debug.Log("Error: " + e.Message);
      return false;
    }
  }
}
