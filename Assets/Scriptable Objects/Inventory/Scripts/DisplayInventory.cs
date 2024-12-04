using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DisplayInventory : MonoBehaviour

{
    public GameObject inventoryPrefab;
    public InventoryObject inventory; // Hold inventory object to display
    Dictionary<InventorySlot, GameObject> itemDisplayed = new Dictionary<InventorySlot, GameObject>();

    // Action
    //public static void 

    // Start is called before the first frame update
    void Start()
    {
        CreateDisplay();
        Player.onItemDrop += RemoveInventorySlot;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateDisplay();
    }

    /// <summary>
    /// Creates the display for the inventory slots of the User Interface and adds it into the dictionary
    /// </summary>
    public void CreateDisplay()
    {
        for (int i = 0; i < inventory.Container.Items.Count; i++)
        {
            InventorySlot slot = inventory.Container.Items[i];

            var obj = Instantiate(inventoryPrefab, Vector3.zero, Quaternion.identity, transform);
            obj.transform.GetChild(0).GetComponentInChildren<Image>().sprite = inventory.database.GetItemById(slot.item.Id).display;
            obj.GetComponentInChildren<TextMeshProUGUI>().text = slot.amount.ToString("n0");
            itemDisplayed.Add(slot, obj);
        }
    }
    /// <summary>
    /// Updates display of inventory during an update.
    /// </summary>
    public void UpdateDisplay()
    {
        for (int i = 0;i < inventory.Container.Items.Count; i++)
        {
            InventorySlot slot = inventory.Container.Items[i];

            if (itemDisplayed.ContainsKey(slot))
            {
                itemDisplayed[slot].GetComponentInChildren<TextMeshProUGUI>().text = slot.amount.ToString("n0");
            } else
            {
                var obj = Instantiate(inventoryPrefab, Vector3.zero, Quaternion.identity, transform);
                obj.transform.GetChild(0).GetComponentInChildren<Image>().sprite = inventory.database.GetItemById(slot.item.Id).display;
                itemDisplayed.Add(slot, obj);
            }
        }
    }

    /// <summary>
    /// Finds
    /// </summary>
    public void RemoveInventorySlot()
    {
        // Remove the item from the dictionary display.
        // Find the inventory slot that has this item and remove that slot.

        // Bug: The gameObject is not destroyed, but the item is removed from the dictionary.
        // Intention: Destroy the related gameObject to the inventory slot. Though the problem is that there is currently no easy identifier to find the gameObject - all save for the sprite.
        // Could try to find if slot matches the sprite of the gameObject, or rahter the sprite of the child of the gameObject.

        Object.Destroy(gameObject.transform.GetChild(0).gameObject);

    }

}
