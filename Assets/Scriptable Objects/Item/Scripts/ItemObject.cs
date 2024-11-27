using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Item types of objects.
/// </summary>
public enum ItemType
{
    Equipment,
    Artifact,
    Default
}

public class ItemObject : ScriptableObject
{
    public int Id;
    public Sprite display;
    public ItemType type;
    public string itemName;
    [TextArea(15, 20)]
    public string description;
}

[System.Serializable]
public class Item
{
    public string Name;
    public int Id;
    public Item(ItemObject item)
    {
        Name = item.itemName;
        Id = item.Id;
    }
}