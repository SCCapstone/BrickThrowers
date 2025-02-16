/*
 * Copyright 2025 Scott Do
 * 2/15/2025
 * This script is used to manage the player's inventory.
 */
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    [SerializeField]
    private GameObject itemCursor; // Where the cursor is for navigation via arrow keys

    [SerializeField]
    private GameObject slotHolder; // The GameObject that holds all the slots prefabs

    [SerializeField]
    private SlotClass[] startingItems; // Starting items that the player should have

    [SerializeField]
    private SlotClass[] items; // Items possessed by the player

    private GameObject[] slots; // how many slots that SlotsHodler has

    public KeyCode inventoryMoveRight = KeyCode.RightShift; // Arrow keys for cursor movement in the inventory
    private const int SLOT_DISTANCE = 110;
    [SerializeField]
    private int currentPos = 0;

    private void Start()
    {
        slots = new GameObject[slotHolder.transform.childCount];
        items = new SlotClass[slots.Length];

        for (int i = 0; i < slots.Length; i++)
        {
            items[i] = new SlotClass();
        }

        for (int i = 0; i < startingItems.Length; i++)
        {
            items[i] = startingItems[i];
        }

        for (int i = 0; i < slotHolder.transform.childCount; i++)
        {
            slots[i] = slotHolder.transform.GetChild(i).gameObject;
        }
    }

    private void Update()
    {
        RefreshUI();
        MoveCursor();
    }

    #region Inventory Utilities
    [ContextMenu("Refresh Inventory")]
    public void RefreshUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            try
            {
                slots[i].transform.GetChild(0).GetComponent<Image>().sprite = items[i]
                    .Item
                    .itemIcon;
                slots[i].transform.GetChild(0).GetComponent<Image>().enabled = true;
            }
            catch
            {
                slots[i].transform.GetChild(0).GetComponent<Image>().sprite = null;
                slots[i].transform.GetChild(0).GetComponent<Image>().enabled = false;
            }
        }
    }

    /// <summary>
    /// Adds an item into a player's inventory.
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool Add(ItemClass item)
    {
        // If all slots have been occupied, then the item cannot be added.
        foreach (SlotClass slotTemp in items)
        {
            if (slotTemp.Item != null)
            {
                slotTemp.Item = item;
                break;
            }
        }
        // check if inventory contains items
        SlotClass slot = Contains(item);

        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].Item == null)
            {
                items[i] = new SlotClass(item);
                break;
            }
        }

        RefreshUI();
        return true;
    }
    /// <summary>
    /// Removes an item from a player's inventory.
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    private ItemClass Remove(ItemClass item)
    {
        int slotToRemove = 0;

        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].Item == item)
            {
                slotToRemove = i;
                break;
            }
        }
        ItemClass temp = items[slotToRemove].Item;
        items[slotToRemove].Clear();

        RefreshUI();
        return temp;
    }

    /*
     * The function is called by the ItemCollectionManager script. When called, this function should remove the item
     * where the current cursor is located. It will then create the GameObject of the item and place it in the world.
     */
    public void DropItem()
    {
        // First, at the cursor, determine if there is an item. If not, disregard the command.
        if (items[currentPos].Item == null)
            return;

        // Else there is an item, remove it from the inventory and drop it in the world.
        // Start by keeping a reference to the item to gain access to the GameObject prefab.
        // Then, instantiate the item in the world.
        ItemClass temp = Remove(items[currentPos].Item);
        // Instantiate the item in the world relative to the player's position; it should be in front of the player.
        Instantiate(temp.prefab, new Vector3(transform.parent.position.x + 1, transform.parent.position.y, transform.parent.position.z), Quaternion.identity);
    }

    public SlotClass Contains(ItemClass item)
    {
        foreach (SlotClass slot in items)
        {
            if (slot != null && item == (slot.Item))
                return slot;
        }
        return null;
    }

    #endregion // Inventory Utilities

    //#region Cusor Moving
    //private bool BeginItemMove()
    //{
    //    originalSlot = GetClosestSlot();
    //    if (originalSlot == null || originalSlot.Item == null)
    //        return false;

    //    movingSlot = new SlotClass(originalSlot);
    //    originalSlot.Clear();
    //    isMovingItem = true;
    //    RefreshUI();
    //    return true;
    //}

    //private SlotClass GetClosestSlot()
    //{
    //    for (int i = 0; i < slots.Length; i++)
    //    {
    //        if (Vector2.Distance(slots[i].transform.position, Input.mousePosition) <= 32)
    //        {
    //            return items[i];
    //        }
    //    }
    //    return null;
    //}

    //private bool EndItemMove()
    //{
    //    originalSlot = GetClosestSlot();
    //    if (originalSlot == null)
    //    {
    //        Add(movingSlot.Item, movingSlot.Quantity);
    //        movingSlot.Clear();
    //    }
    //    else
    //    {
    //        if (originalSlot.Item != null)
    //        {
    //            if (originalSlot.Item.Equals(movingSlot.Item)) // same item, should stack
    //            {
    //                if (originalSlot.Item.isStackable)
    //                {
    //                    originalSlot.Quantity += movingSlot.Quantity;
    //                    movingSlot.Clear();
    //                }
    //                else
    //                    return false;
    //            }
    //            else
    //            {
    //                //swap them
    //                tempSlot = new SlotClass(originalSlot);
    //                originalSlot.Item = movingSlot.Item;
    //                movingSlot.Item = tempSlot.Item;
    //                RefreshUI();
    //                return true;
    //            }
    //        }
    //        else
    //        {
    //            // place item there
    //            originalSlot.Item = movingSlot.Item;
    //            movingSlot.Clear();
    //        }
    //    }
    //    isMovingItem = false;
    //    RefreshUI();
    //    return true;
    //}
    //#endregion

    #region Arrow Cursor Navigation
    // quote

    private bool MoveCursor()
    {
        if(Input.GetKeyDown(inventoryMoveRight))
        {
            // With each click, the cursor moves right. As it stands, there are three inventory slots. 
            // When the third slot has been reached and the right arrow key is pressed, the cursor will move to the first slot.
            itemCursor.transform.position = new Vector2(itemCursor.transform.position.x + SLOT_DISTANCE, itemCursor.transform.position.y);
            currentPos++;

            if (currentPos > 2)
            {
                itemCursor.transform.position = new Vector2(itemCursor.transform.position.x - (SLOT_DISTANCE * 3), itemCursor.transform.position.y);
                currentPos = 0;
            }
        }
        return true;
    }

    #endregion
}
