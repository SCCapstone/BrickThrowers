using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Porter : MonoBehaviour
{
    private object extraSlotItem; // The extra slot for the Porter variant!

    public bool HasExtraSlotItem => extraSlotItem != null;

    public bool AddToExtraSlot(object item)
    {
        if (extraSlotItem == null) 
        {
            extraSlotItem = item;
            Debug.Log("Item added to Porter's extra slot.");
            return true;
        }
        Debug.Log("Extra slot is already occupied.");
        return false;
    }

    public object RemoveFromExtraSlot()
    {
        if (extraSlotItem != null)
        {
            object itemToReturn = extraSlotItem;
            extraSlotItem = null;
            Debug.Log("Item removed from Porter's extra slot.");
            return itemToReturn;
        }
        Debug.Log("Extra slot is empty.");
        return null;
    }

    // ths gets the item in the extra slot
    public object GetExtraSlotItem()
    {
        return extraSlotItem;
    }
}
