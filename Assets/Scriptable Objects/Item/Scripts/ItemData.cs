using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Equipment,
    Artifact,
    Default
}

public class ItemData : ScriptableObject
{
    public GameObject prefab;
    public ItemType type;
    public string itemName;
    [TextArea(15, 20)]
    public string description;
}
