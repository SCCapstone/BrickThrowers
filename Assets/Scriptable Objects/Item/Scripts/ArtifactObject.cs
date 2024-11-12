/// Create an Artifact pickup item
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Artifact Object", menuName = "Inventory System/Items/Artifact")]
public class ArtifactObject : ItemData
{
    public int value; /// Value of artifact

    public void Awake()
    {
        type = ItemType.Artifact;
    }
}
