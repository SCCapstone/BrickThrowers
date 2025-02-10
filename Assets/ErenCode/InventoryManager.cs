using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    [SerializeField]
    private GameObject itemCursor;

    [SerializeField]
    private GameObject slotHolder;

    [SerializeField]
    private ItemClass itemToAdd;

    [SerializeField]
    private ItemClass itemToRemove;

    [SerializeField]
    private SlotClass[] startingItems;

    private SlotClass[] items;

    private GameObject[] slots; // how many slots that the GameObject has

    private SlotClass tempSlot;
    private SlotClass movingSlot;
    private SlotClass originalSlot;
    private bool isMovingItem;

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

        RefreshUI();

        Add(itemToAdd);
        ///Remove(itemToRemove);
    }

    private void Update()
    {
        itemCursor.SetActive(isMovingItem);
        itemCursor.transform.position = Input.mousePosition;
        if (isMovingItem)
        {
            itemCursor.GetComponent<Image>().sprite = movingSlot.Item.itemIcon;
        }


        if (Input.GetMouseButtonDown(0))
        {
            //Debug.Log(GetClosestSlot().Item);
            //if (isMovingItem)
            //{
            //    EndItemMove();
            //}
            //else
            //    BeginItemMove();
        }
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

    public bool Add(ItemClass item)
    {
        // check if inventory contains items
        SlotClass slot = Contains(item);

        // If there is not a null slot and is stackable, then add another item.
        //if (slot != null && item.isStackable)
        //    slot.Quantity += 1;
        //else
        //{
        //    for (int i = 0; i < items.Length; i++)
        //    {
        //        if (items[i].Item == null)
        //        {
        //            items[i] = new SlotClass(item, quantity);
        //            break;
        //        }
        //    }
        //}

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

    public bool Remove(ItemClass item)
    {
        //SlotClass temp = Contains(item);
        //if (!temp.Equals(null))
        //{
        //    if (temp.Quantity >= 1)
        //        temp.Quantity -= 1;
        //    else
        //    {
        //        int slotToRemove = 0;

        //        for (int i = 0; i < items.Length; i++)
        //        {
        //            if (items[i].Item.Equals(item))
        //            {
        //                slotToRemove = i;
        //                break;
        //            }
        //        }

        //        items[slotToRemove].Clear();
        //    }
        //}
        //else
        //{
        //    return false;
        //}

        int slotToRemove = 0;

        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].Item.Equals(item))
            {
                slotToRemove = i;
                break;
            }
        }

        items[slotToRemove].Clear();

        RefreshUI();
        return true;
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
}
