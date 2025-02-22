/*
 * Copyright 2025 Scott Do
 * 2/15/2025
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemClass : ScriptableObject
{
    public enum ItemType
    {
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

    // Every item class that is a child of this need to return an item.
    public abstract ItemClass GetItem();

    /// <summary>
    /// Uses the item.
    /// </summary>
    /// <param name="player"></param>
    /// <returns>True if the item can be used, otherwise false.</returns>
    public abstract bool Use(Player player);

    /// <summary>
    /// Run this when calling for an artifact's value.
    /// </summary>
    /// <returns></returns>
    public virtual int GetValue()
    {
        return 0;
    }
}
