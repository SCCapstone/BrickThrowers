using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment Object", menuName = "Inventory System/Items/Equipment")]
public class EquipmentObject : ItemData
{
    // Function instance value here
    public Item function;
    public void Awake()
    {
        type = ItemType.Equipment;
    }
}
