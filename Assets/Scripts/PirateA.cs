// Copyright 2025 Brick Throwers
// PirateA.cs - Controls the behavior of PirateA, including movement, attacking, and stealing artifacts.
// Not in use.
using UnityEngine;

public class PirateA : MonoBehaviour {
  public float speed = 4f;                 // Movement speed
  public float attackRange = 2f;          // Range to attack player
  public float artifactStealRange = 1.5f; // Range to steal artifacts
  public int health = 50;                 // Health of the pirate
  public int damage = 10;                 // Damage dealt to the player

  private Transform targetPlayer;         // Current target (player)
  private bool hasArtifact = false;       // Whether the pirate has stolen an artifact
  private Rigidbody2D rb;

  void Start() {
    rb = GetComponent<Rigidbody2D>();

    // Find the player to target (assuming tag "Player")
    GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
    if (playerObject != null) {
      targetPlayer = playerObject.transform;
    }
  }

  void Update() {
    if (targetPlayer != null) {
      MoveTowardsTarget();

      float distance = Vector2.Distance(transform.position, targetPlayer.position);

      // Check for artifact stealing
      if (distance <= artifactStealRange && !hasArtifact) {
        StealArtifact(targetPlayer.gameObject);
      }

      // Check for attacking the player
      if (distance <= attackRange) {
        AttackPlayer(targetPlayer.gameObject);
      }
    }
  }

  void MoveTowardsTarget() {
    Vector2 direction = ((Vector2)targetPlayer.position - (Vector2)transform.position).normalized;
    rb.velocity = direction * speed;
  }

  void StealArtifact(GameObject player) {
    Player playerComponent = player.GetComponent<Player>();
    if (playerComponent != null && playerComponent.HasArtifact()) {
      hasArtifact = true;
      playerComponent.RemoveArtifact();
      Debug.Log("PirateA stole an artifact!");
    }
  }

  void AttackPlayer(GameObject player) {
    Player playerComponent = player.GetComponent<Player>();
    if (playerComponent != null) {
      playerComponent.TakeOxygenDamage(damage);
      Debug.Log("PirateA attacked the player!");
    }
  }

  public void TakeDamage(int damageAmount) {
    health -= damageAmount;
    if (health <= 0) {
      DropArtifact();
      Destroy(gameObject);
    }
  }

  void DropArtifact() {
    if (hasArtifact) {
      Debug.Log("PirateA dropped an artifact!");
      // Implement artifact drop logic
    }
  }
}
