using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scripts : ScriptableObject
{
    public List<ItemData> Container = new List<ItemData>();
}

[System.Serializable]
public class InventorySlot
{
    public ItemData item;
    public int amount;
    public InventorySlot(Item)
}