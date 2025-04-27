// Copyright 2025 Brick Throwers
// // AttackSensor.cs - Detects enemies in range and applies damage when attacked
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSensor : MonoBehaviour {
  // References
  public List<GameObject> enemiesInRange = new List<GameObject>();
  private const int ATTACK_DMG = 15;
  [SerializeField] private Harpooner.Direction zoneDirection;

  #region Setup Functions
  private void OnEnable() {
    Harpooner.onAttack += HandleAttack;
  }

  private void OnDisable() {
    Harpooner.onAttack -= HandleAttack;
  }
  /// <summary>
  /// Calls attack function if the attack direction matches the zone direction.
  /// </summary>
  /// <param name="dir"></param>
  private void HandleAttack(Harpooner.Direction dir) {
    if (dir == zoneDirection)
      Attack();
  }
  #endregion

  #region Collision Logic
  private void OnTriggerEnter2D(Collider2D collision) {
    if (collision.CompareTag("Enemy")) {
      enemiesInRange.Add(collision.gameObject);
      Debug.Log("Enemy entered range: " + collision.name);
    }
  }

  private void OnTriggerExit2D(Collider2D collision) {
    if (collision.CompareTag("Enemy")) {
      enemiesInRange.Remove(collision.gameObject);
      Debug.Log("Enemy exited range: " + collision.name);
    }
  }
  #endregion
  /// <summary>
  /// Attacks all enemies in range.
  /// </summary>
  private void Attack() {
    // 1) Remove destroyed entries
    enemiesInRange.RemoveAll(e => e == null);

    // 2) If empty, nothing to do
    if (enemiesInRange.Count == 0) {
      Debug.Log("No enemies in range to attack.");
      return;
    }

    // 3) Iterate safely over a snapshot
    foreach (var enemy in enemiesInRange.ToArray()) {
      var damageable = enemy.GetComponent<IDamageable>();
      if (damageable != null) {
        damageable.TakeDamage(ATTACK_DMG);
        Debug.Log($"Attacked {enemy.name} for {ATTACK_DMG} damage.");
      }
    }
  }

}
