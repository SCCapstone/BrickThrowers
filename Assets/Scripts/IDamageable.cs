using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface for all enemy classes to implement. They all take damage.
/// </summary>
public interface IDamageable {
  void TakeDamage(int amount);
}
