/*
 * Copyright 2025 Scott Do
 * 2/15/2025
 */
using UnityEngine;

/// <summary>
/// Database object.
/// </summary>
[CreateAssetMenu(fileName = "Item Database", menuName = "Inventory/Items/Database")]
public class ItemDatabaseObject : ScriptableObject, ISerializationCallbackReceiver
{
    public ItemClass[] itemsDatabase; // list of all items in game

    /// <summary>
    /// Deseralize this data object to something useful for Unity to use, like a database.
    /// </summary>
    public void OnAfterDeserialize()
    {
        for (int i = 0; i < itemsDatabase.Length; i++)
        {
            itemsDatabase[i].itemID = i;
        }
    }

    public void OnBeforeSerialize() { }
}
