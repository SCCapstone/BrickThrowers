using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Equipment", menuName = "Items/Equipment")]
public class EquipmentClass : ItemClass
{
    [Header("Tool")]
    public ToolType toolType;
    public enum ToolType
    {
        flashlight,
    }
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
