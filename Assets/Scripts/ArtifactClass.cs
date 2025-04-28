// Copyright 2025 Brick Throwers
// ArtifactClass.cs - Defines the ArtifactClass for the game.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Artifacts", menuName = "Items/Artifacts")]
public class ArtifactClass : ItemClass {
  // Artifact specific classes
  [Header("Artifact Details")]
  public ArtifactType artifactType;
  public int value;

  /// <summary>
  /// Rarity of artifacts.
  /// </summary>
  public enum ArtifactType {
    unique,
    rare,
    common,
    basic,
  }
  /// <summary>
  /// Return the item.
  /// </summary>
  /// <returns></returns>
  public override ItemClass GetItem() {
    return this;
  }
  /// <summary>
  /// Use function cannot be used for artifacts.
  /// </summary>
  /// <param name="player"></param>
  /// <returns></returns>
  public override bool Use(Player player) {
    // Artifacts cannot be used
    return false;
  }
}
