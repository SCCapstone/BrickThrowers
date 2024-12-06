using UnityEngine;

/// <summary>
/// Database object.
/// </summary>
[CreateAssetMenu(fileName = "Item Database", menuName = "Inventory System/Items/Database")]
public class ItemDatabaseObject : ScriptableObject, ISerializationCallbackReceiver
{
    public ItemObject[] items; // list of all items in game
    //public Dictionary<ItemObject, int> GetID = new Dictionary<ItemObject, int>(); // dictionary of items and their ID

    /// <summary>
    /// Deseralize this data object to something useful for Unity to use, like a database.
    /// </summary>
    public void OnAfterDeserialize()
    {
        for (int i = 0; i < items.Length; i++)
        {
            items[i].Id = i;
        }
    }
    /// <summary>
    /// Return the item by its ID.
    /// </summary>
    /// <param name="_id"></param>
    /// <returns></returns>
    public ItemObject GetItemById(int _id)
    {
        if (_id >= items.Length)
        {
            return null;
        }

        return items[_id];

    }

    public void OnBeforeSerialize()
    {

    }
}
