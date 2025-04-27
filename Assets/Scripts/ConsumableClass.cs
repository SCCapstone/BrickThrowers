/*
 * Copyright 2025 Brick Throwers
 * Done by Reshlynt (Scott Do) 2/15/2025
 * Class for consumable items.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Consumable", menuName = "Items/Consumable")]
public class ConsumableClass : ItemClass {
  [Header("Consumable")]
  public ConsumableType consumableType;

  public enum ConsumableType {
    antidote,
  }

  public override ItemClass GetItem() {
    return this;
  }

  public override bool Use(Player player) {
    if (consumableType == ConsumableType.antidote) {
      player.RelievePoison();
      return true;
    }
    // More use cases.


    // At this point, none of the use cases were met. The item is not used.
    return false;
  }
}
