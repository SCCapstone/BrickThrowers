/*
 * Copyright 2025 Scott Do
 * 2/15/2025
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Artifacts", menuName = "Items/Artifacts")]
public class ArtifactClass : ItemClass
{
    // Artifact specific classes
    [Header("Artifact Details")]
    public ArtifactType artifactType;
    public int value;

    public enum ArtifactType
    {
        rare,
        common,
        garbage,
    }

    // Action subscription
    //private void OnEnable()
    //{
    //    ArtifactCollecter.onPlayerCollectArtifact += ChangeDropStatus;
    //}

    //private void OnDisable()
    //{
    //    ArtifactCollecter.onPlayerCollectArtifact -= ChangeDropStatus;
    //}
    public override ItemClass GetItem()
    {
        return this;
    }

    public override bool Use(Player player)
    {
        // Artifacts cannot be used
        return false;
    }
}
