/*
 * Copyright 2025 Scott Do
 * 2/15/2025
 * Defines the item class for the game.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemClass : ScriptableObject {
  public enum ItemType {
    artifact,
    equipment,
    consumable,
  }

  // Things every item has.
  [Header("Item")]
  public int itemID;
  public string itemName;
  public Sprite itemIcon;
  public GameObject prefab; // The GameObject prefab concerning with the item
  public ItemType itemType;
  [SerializeField] private int cost; // By default, artifacts are not for sale and cost 0.

  /// <summary>
  /// Returns the item.
  /// </summary>
  /// <returns></returns>
  public abstract ItemClass GetItem();

  /// <summary>
  /// Uses the item.
  /// </summary>
  /// <param name="player"></param>
  /// <returns>True if the item can be used, otherwise false.</returns>
  public abstract bool Use(Player player);

  public int ReturnCost() {
    if (itemType == ItemType.artifact) {
      return 0;
    } else {
      return cost;
    }
  }
}
