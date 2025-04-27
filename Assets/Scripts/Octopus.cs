// Copyright 2025 Brick Throwers
// // Octopus.cs - Defines the Octopus enemy class.
using System.Collections;
using UnityEngine;

public class Octopus : MonoBehaviour, IDamageable {
  public float detectionRange = 3f; // Range to detect the player
  public float latchDuration = 5f; // Default time to free from latch
  public int health = 60; // Health of the octopus
  public float moveSpeed = 15f; // Movement speed when roaming

  private Transform targetPlayer; // Player being targeted
  private Rigidbody2D rb;
  private bool isLatched = false; // Whether the octopus is latched onto the player
  private float currentLatchTime; // Remaining time for latch release
  private int decayMultiplier = 1; // Multiplier for latch decay

  private bool isRoaming = false;
  [SerializeField] private float roamDuration = 2f;

  void Start() {
    rb = GetComponent<Rigidbody2D>();
    Roam();
  }

  void Update() {
    //if (!isLatched)
    //{
    //    // Random roaming behavior

    //}
    if (!isRoaming) {
      StartCoroutine(Roam());
    }
  }
  #region Roam Logic
  /// <summary>
  /// Roam the octopus in a random direction for a set duration.
  /// </summary>
  /// <returns></returns>
  IEnumerator Roam() {
    isRoaming = true;
    Vector2 randomDirection = Random.insideUnitCircle.normalized;
    rb.velocity = randomDirection * moveSpeed;
    yield return new WaitForSeconds(roamDuration);
    isRoaming = false;

  }
  
  public void TakeDamage(int damageAmount) {
    health -= damageAmount;
    if (health <= 0) {
      Destroy(gameObject);
      Debug.Log("Octopus defeated!");
    }
  }
  #endregion
  #region Latch Logic
  // Called when the octopus collides with the player
  private void OnTriggerEnter2D(Collider2D collision) {
    if (collision.gameObject.CompareTag("Player")) {
      Player player = collision.gameObject.GetComponent<Player>();

      if (player != null) {
        // Latching logic for the player
        StartCoroutine(LatchOntoPlayer(player, collision.gameObject.transform));
      }
    }
  }
  /// <summary>
  /// Latch onto the player and suppress their movement and light.
  /// </summary>
  /// <param name="player"></param>
  /// <param name="playerTransform"></param>
  private void Latch(Player player, Transform playerTransform) {
    player.SuppressMovement(true);
    player.SuppressLight(true);
    Debug.Log("Octopus latch onto player.");
    // Have the octopus game object cling onto the player
    // while the latch timer is running
    this.transform.SetParent(playerTransform);
    rb.constraints = RigidbodyConstraints2D.FreezePositionX;
    rb.drag = 1f;
    rb.gravityScale = 1f;
  }

  /// <summary>
  /// Coroutine to latch onto the player for a set duration.
  /// </summary>
  /// <param name="player"></param>
  /// <param name="playerTransform"></param>
  /// <returns></returns>
  IEnumerator LatchOntoPlayer(Player player, Transform playerTransform) {
    isLatched = true;
    currentLatchTime = latchDuration;
    Latch(player, playerTransform);
    while (currentLatchTime > 0f) {
      currentLatchTime -= Time.deltaTime * decayMultiplier;
      yield return null;
    }
    Release(player);
    Vector2 direction = (transform.position - player.transform.position).normalized;
    rb.velocity = direction * moveSpeed;
  }
  /// <summary>
  /// Release the player from the octopus latch.
  /// </summary>
  /// <param name="player"></param>
  void Release(Player player) {
    isLatched = false;
    player.SuppressMovement(false);
    player.SuppressLight(false);
    this.transform.SetParent(null);
    Debug.Log("Player freed from the octopus!");
    rb.constraints = RigidbodyConstraints2D.None;
    rb.drag = 0f; // Reset drag to normal
    rb.gravityScale = 0f;
  }
  #endregion
}
