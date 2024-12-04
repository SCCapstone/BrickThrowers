using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEngine;
/// <summary>
/// Stores the list of items in the inventory, and manages inventory slots.
/// </summary>
[CreateAssetMenu(fileName = "New Inventory Object", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject
{
    public ItemDatabaseObject database;
    public string savePath;
    public Inventory Container;
    public const int LIMIT = 3;

    // Add item to inventory
    // Remember that it currently is able to stack
    // Will have to remove later
    public bool AddItem(Item _item, int _amount)
    {
        //for (int i = 0; i < Container.Items.Count; i++)
        //{
        //    if (Container.Items[i].item.Id == _item.Id)
        //    {
        //        Container.Items[i].AddAmount(_amount);
        //        return;
        //    }
        //}
        if(Container.Items.Count >= LIMIT)
        {
            Debug.Log("Inventory is full");
            return false;
        }
        Container.Items.Add(new InventorySlot(_item.Id, _item, _amount));
        return true;
    }
    // For now it will have to remove the first item on the list.
    public Item RemoveItem()
    {
        // Check if the inventory is empty
        // If true, return since there is nothing to do
        if (Container.Items.Count == 0)
        {
            return null;
        }
        // Take the first item on the list
        // Remove the first item on the list
        Item item = Container.Items[0].item;
        Container.Items.RemoveAt(0);

        // Find that item that appears in the inventory slot list and remove the slot
        Container.Items.Remove(Container.Items.Find(x => x.item == item));

        return item;

    }

    /// <summary>
    /// Save the inventory to a file
    /// </summary>
    [ContextMenu("Save")]
    public void Save()
    {
        // JSON
        string saveData = JsonUtility.ToJson(this, true);
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(string.Concat(Application.persistentDataPath, savePath));
        bf.Serialize(file, saveData);
        file.Close();

        ////IFormatter
        //IFormatter formatter = new BinaryFormatter();
        //Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Create, FileAccess.Write);
        //formatter.Serialize(stream, Container);
        //stream.Close();
    }
    /// <summary>
    /// Load the inventory from a file
    /// </summary>
    [ContextMenu("Load")]
    public void Load()
    {
        if (File.Exists(string.Concat(Application.persistentDataPath, savePath)))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(string.Concat(Application.persistentDataPath, savePath), FileMode.Open);
            JsonUtility.FromJsonOverwrite(bf.Deserialize(file).ToString(), this);
            file.Close();

            //Iformatter
            //IFormatter formatter = new BinaryFormatter();
            //Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Open, FileAccess.Read);
            //Container = (Inventory)formatter.Deserialize(stream);
            //stream.Close();
        }
    }

    [ContextMenu("Clear")]
    public void Clear()
    {
        Container.Items.Clear();
    }
}
/// <summary>
/// A class that holds the inventory of the player.
/// </summary>
[System.Serializable]
public class Inventory
{
    public List<InventorySlot> Items = new List<InventorySlot>();
}

/// <summary>
///  Create inventory slots
/// </summary>
[System.Serializable]
public class InventorySlot
{
    public int ID;
    public Item item;
    public int amount;
    public InventorySlot(int _id, Item _item, int _amount)
    {
        ID = _id;
        item = _item;
        amount = _amount;
    }
}