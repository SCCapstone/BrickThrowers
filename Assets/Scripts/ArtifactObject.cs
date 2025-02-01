/// Create an Artifact pickup item
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Artifact Object", menuName = "Inventory System/Items/Artifact")]
public class ArtifactObject : ItemObject
{
    public int value; /// Value of artifact
    // Awake means that the internal actions will run when the object is created
    public void Awake()
    {
        type = ItemType.Artifact;
    }
}
