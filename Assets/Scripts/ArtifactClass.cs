using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Artifacts", menuName = "Items/Artifacts")]
public class ArtifactClass : ItemClass
{
    // Artifact specific classes
    [Header("Misc")]
    public ArtifactType artifactType;

    public enum ArtifactType
    {
        rare,
        common,
        garbage
    }
    public override ItemClass GetItem()
    {
        return this;
    }
}
