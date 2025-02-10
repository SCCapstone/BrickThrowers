using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Tool", menuName = "ErenCore/Item/Tool")]
public class ToolClass : ItemClass
{
    [Header("Tool")]
    public ToolType toolType;
    public enum ToolType
    {
        flashlight,
        harpoon
    }
    public override ItemClass GetItem()
    {
        return this;
    }

    public ToolClass GetTool()
    {
        return this;
    }
}
