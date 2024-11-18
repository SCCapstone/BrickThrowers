using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Database object.
/// </summary>
[CreateAssetMenu(fileName = "Item Database", menuName = "Inventory System/Items/Database")]
public class ItemDatabaseObject : ScriptableObject, ISerializationCallbackReceiver
{
    public ItemObject[] Items; // list of all items in game
    // Take a memory hit for performance
    public Dictionary<ItemObject, int> GetID = new Dictionary<ItemObject, int>(); // dictionary of items and their ID
    public Dictionary<int, ItemObject> GetItem = new Dictionary<int, ItemObject>();

    /// <summary>
    /// Deseralize this data object to something useful for Unity to use, like a database.
    /// </summary>
    public void OnAfterDeserialize()
    {
        GetID = new Dictionary<ItemObject, int>();
        for (int i = 0; i < Items.Length; i++)
        {
            GetID.Add(Items[i], i);
            GetItem.Add(i, Items[i]);
        }
    }

    public void OnBeforeSerialize()
    {

    }
}
