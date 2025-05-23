// Copyright 2025 Brick Throwers
// Lionfish.cs - Controls the lionfish enemy behavior.
using UnityEngine;
public class Lionfish : MonoBehaviour, IDamageable {
  public float patrolSpeed = 2f;               // Speed of patrol movement
  public float directionChangeInterval = 3f;  // Time between direction changes
  public int health = 40;                     // Health of the lionfish

  private Rigidbody2D rb;
  private Vector2 patrolDirection;
  private float directionChangeTimer;

  void Start() {
    rb = GetComponent<Rigidbody2D>();
    ChangeDirection();
    directionChangeTimer = directionChangeInterval;
  }

  void Update() {
    directionChangeTimer -= Time.deltaTime;

    if (directionChangeTimer <= 0f) {
      ChangeDirection();
      directionChangeTimer = directionChangeInterval;
    }

    Patrol();
  }
  /// <summary>
  /// Changes the lionfish's patrol direction randomly.
  /// </summary>
  void ChangeDirection() {
    float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
    patrolDirection = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).normalized;
  }
  /// <summary>
  /// Patrols the lionfish in the current direction.
  /// </summary>
  void Patrol() {
    rb.velocity = patrolDirection * patrolSpeed;
  }

  void OnCollisionEnter2D(Collision2D collision) {
    if (collision.gameObject.CompareTag("Player")) {
      Diver diver = collision.gameObject.GetComponent<Diver>();
      if (diver != null) {
        diver.ApplyPoison();
        Debug.Log("Lionfish attacked the diver!");
      }
    }
  }

  public void TakeDamage(int damageAmount) {
    health -= damageAmount;
    if (health <= 0) {
      Destroy(gameObject);
      Debug.Log("Lionfish defeated!");
    }
  }
}
