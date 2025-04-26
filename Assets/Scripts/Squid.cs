using UnityEngine;

public class Squid : MonoBehaviour {
  public float ambushSpeed = 7f;              // Speed during ambush
  public float retreatSpeed = 10f;           // Speed during retreat
  public float ambushRange = 8f;             // Detection range for ambush
  public float inkBlindDuration = 3f;        // Duration of blindness effect
  public float retreatDuration = 1.5f;       // Duration of retreat after ambush

  private Transform targetPlayer;            // Targeted player
  private Vector2 retreatDirection;
  private bool isAmbushing = false;
  private bool isRetreating = false;
  private float retreatTimer = 0f;

  private Rigidbody2D rb;

  void Start() {
    GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
    if (playerObject != null) {
      targetPlayer = playerObject.transform;
    } else {
      Debug.LogError("No GameObject with tag 'Player' found in the scene.");
    }

    rb = GetComponent<Rigidbody2D>();
    if (rb == null) {
      Debug.LogError("Rigidbody2D is missing on the Squid GameObject.");
    }
  }

  void Update() {
    if (targetPlayer == null || rb == null) return;

    if (isAmbushing) {
      AmbushPlayer();
    } else if (isRetreating) {
      retreatTimer -= Time.deltaTime;
      if (retreatTimer <= 0f) {
        isRetreating = false;
      }
      Retreat();
    } else {
      DetectPlayerAndAmbush();
    }
  }

  void DetectPlayerAndAmbush() {
    // Start ambush if the player is within ambush range
    if (Vector2.Distance(transform.position, targetPlayer.position) <= ambushRange) {
      isAmbushing = true;
    }
  }

  void AmbushPlayer() {
    // Move toward the player quickly
    Vector2 directionToPlayer = (targetPlayer.position - transform.position).normalized;
    rb.MovePosition(rb.position + directionToPlayer * ambushSpeed * Time.deltaTime);

    // If close enough to the player, release ink and retreat
    if (Vector2.Distance(transform.position, targetPlayer.position) <= 1f) {
      ReleaseInk();
      StartRetreat(directionToPlayer);
    }
  }

  void StartRetreat(Vector2 directionToPlayer) {
    isAmbushing = false;
    isRetreating = true;
    retreatDirection = -directionToPlayer; // Retreat in the opposite direction
    retreatTimer = retreatDuration;
  }

  void Retreat() {
    rb.MovePosition(rb.position + retreatDirection * retreatSpeed * Time.deltaTime);
  }

  void ReleaseInk() {
    Debug.Log("Squid releases ink, blinding the player!");
    Player player = targetPlayer.GetComponent<Player>();
    if (player != null) {
      player.Blind(inkBlindDuration);
    } else {
      Debug.LogError("The target does not have a Player component!");
    }
  }
}
