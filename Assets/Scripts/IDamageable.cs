// Copyright 2025 Brick Throwers
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface for all enemy classes to implement. They all take damage.
/// </summary>
public interface IDamageable {
  /// <summary>
  /// Take damage from player attack.
  /// </summary>
  /// <param name="amount"></param>
  void TakeDamage(int amount);
}
