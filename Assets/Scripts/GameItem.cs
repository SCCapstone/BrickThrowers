/*
 * Copyright 2025 Scott Do
 * 2/15/2025
 * Allows for the ItemClass to be used in the game.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameItem : MonoBehaviour {
  // The type of item associated with every collectable that appears in the game.
  public ItemClass gameItemClass;
  private bool inDropZone = false;

  // Defunct code
  public bool InDropZone {
    get {
      return inDropZone;
    }
    set {
      if (gameItemClass == null) {
        Debug.LogWarning("gameItemClass is null, cannot set InDropZone.");
        return;
      }
      if (gameItemClass.itemType == ItemClass.ItemType.artifact) {
        inDropZone = value;
      } else {
        Debug.LogWarning("Attempted to set InDropZone on a non-artifact item.");
      }
    }

  }

}
