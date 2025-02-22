/*
 * Copyright 2025 Scott Do
 * 2/15/2025
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Equipment", menuName = "Items/Equipment")]
public class EquipmentClass : ItemClass
{
    public override ItemClass GetItem()
    {
        return this;
    }

    public EquipmentClass GetTool()
    {
        return this;
    }

    public override bool Use(Player player)
    {
        throw new System.NotImplementedException();
    }
}
