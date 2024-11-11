using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    private Dictionary<InventoryItemData, InventoryItem> itemDictionary;
    public List<InventoryItem> inventory { get; private set; }

    private void Awake()
    {
        itemDictionary = new Dictionary<InventoryItemData, InventoryItem>();
        inventory = new List<InventoryItem>();
    }
    /// add inventory item to inventory
    public void Add(InventoryItemData)
    {
        
    }
}
