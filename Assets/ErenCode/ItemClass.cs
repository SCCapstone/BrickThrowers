using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class ItemClass : ScriptableObject
{
    // Things every item has.
    [Header("Item")]
    public int itemID;
    public string itemName;
    public Sprite itemIcon;

    // Every item class that is a child of this need to return an item.
    public abstract ItemClass GetItem();
    
}