/*
 * Copyright 2025 Scott Do
 * 2/15/2025
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Consumable", menuName = "Items/Consumable")]
public class ConsumableClass : ItemClass
{
    [Header("Consumable")]
    public ConsumableType consumableType;

    public enum ConsumableType
    {
        antidote,
    }

    public override ItemClass GetItem()
    {
        return this;
    }

    public override bool Use(Player player)
    {
        if (consumableType == ConsumableType.antidote)
        {
            return RelievePoison(player);
        }
        // More use cases.


        // At this point, none of the use cases were met. The item is not used.
        return false;
    }

    /// <summary>
    /// Relieves the poision effect from the player.
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    private bool RelievePoison(Player player)
    {
        if (player.isPoisoned)
        {
            player.isPoisoned = false;
            Debug.Log("Player used " + itemName + " and is no longer poisoned.");
            return true;
        }
        else
        {
            Debug.Log("Player used " + itemName + " but is not poisoned.");
            return false;
        }
    }
}
