using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class InventoryItem 
{
    InventoryItemData itemData;

    // Function program later.

    public InventoryItem(InventoryItemData source)
    {
        this.itemData = source;
    }
}
