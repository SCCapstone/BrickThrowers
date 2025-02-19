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
    public GameObject prefab; // The GameObject prefab concerning with the item

    // Every item class that is a child of this need to return an item.
    public abstract ItemClass GetItem();

    // Every item has a function to benefit the player.
    // This function is called with the player intends to use an item.
    public virtual bool Use(Player player)
    {
        Debug.Log(itemName + " used");
        return true;
    }

}