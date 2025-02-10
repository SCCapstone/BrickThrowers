using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Misc", menuName = "ErenCore/Item/Misc")]
public class MiscClass : ItemClass
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

    public MiscClass GetMisc()
    {
        return this;
    }
}
