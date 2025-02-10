using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Consumable", menuName = "ErenCore/Item/Consumable")]
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

    public ConsumableClass GetConsumable()
    {
        return this;
    }
}
