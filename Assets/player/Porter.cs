// kjthao
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Porter : Player
{
    private int extraInventorySlots = 1;

    public override void ChangeToPorter()
    {
        // Change sprite to Porter sprite
        spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/Porter");  // Adjust path to sprite

        // Add extra inventory slot
        AddExtraInventorySlot();
        Debug.Log("Player is now a Porter!");
    }

    void AddExtraInventorySlot()
    {
        InventoryObject inventory = GetComponent<InventoryObject>();
        if (inventory != null)
        {
            inventory.Container.Items.Capacity += extraInventorySlots;
            Debug.Log("Porter gains extra inventory slot.");
        }
        else
        {
            Debug.LogWarning("InventoryObject not found on this GameObject.");
        }
    }
}
