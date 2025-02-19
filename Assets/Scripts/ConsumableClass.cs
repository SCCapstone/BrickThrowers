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
        antidote
    }
    public override ItemClass GetItem()
    {
        return this;
    }

    public override bool Use(Player player)
    {
        // If the player is poisoned, use the antidote.
        if (consumableType == ConsumableType.antidote && player.isPoisoned)
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
