using UnityEngine;

public class Anglerfish : MonoBehaviour, IDamageable {
  // Anglerfish properties
  public float swimSpeed = 2f;                 // Speed of anglerfish movement
  public float directionChangeInterval = 5f;  // Time between direction changes
  public Light anglerLight;                   // The anglerfish's light
  public float lightIntensity = 3f;           // Maximum light intensity
  public float flickerFrequency = 0.1f;       // Frequency of light flicker
  public float detectionRange = 5f;           // Range at which the anglerfish interacts with the player
  public float oxygenDamage = 0.2f;               // Amount of oxygen damage to apply to the player
  public int health = 30;



  private Rigidbody2D rb;
  private Vector2 swimDirection;
  private float directionChangeTimer;
  private Transform targetPlayer;

  void Start() {
    rb = GetComponent<Rigidbody2D>();
    if (rb == null) {
      Debug.LogError("No Rigidbody2D attached to the Anglerfish GameObject.");
      return;
    }

    if (anglerLight != null) {
      InvokeRepeating(nameof(FlickerLight), 0f, flickerFrequency); // Start light flickering
    }

    swimDirection = GetRandomDirection();
    directionChangeTimer = directionChangeInterval;

    // Find the player in the scene
    GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
    if (playerObject != null) {
      targetPlayer = playerObject.transform;
    } else {
      Debug.LogError("No GameObject with tag 'Player' found in the scene.");
    }
  }

  void Update() {
    if (rb == null) return;

    // Timer for changing swim direction
    directionChangeTimer -= Time.deltaTime;
    if (directionChangeTimer <= 0f) {
      swimDirection = GetRandomDirection();
      directionChangeTimer = directionChangeInterval;
    }

    // Move the anglerfish
    rb.velocity = swimDirection * swimSpeed;
  }

  void FlickerLight() {
    if (anglerLight != null) {
      anglerLight.intensity = Random.Range(0.5f, lightIntensity); // Randomize light intensity
    }
  }

  Vector2 GetRandomDirection() {
    float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
    return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).normalized;
  }

  // Damage logic.
  private void OnCollisionStay2D(Collision2D collision) {
    if (collision == null) return;

    /*
     * Determine if the game object that has collided with the anglerfish is the player.
     * The Player game object has the Player tag.
     * If the collisiding game object is the player, call the function to damage the player in Diver.cs
     */
    if (collision.gameObject.CompareTag("Player")) {
      Diver diver = collision.gameObject.GetComponent<Diver>();
      if (diver != null) {
        diver.TakeOxygenDamage(oxygenDamage);
        Debug.Log("Anglerfish damaged the player's oxygen!");
      } else {
        Debug.LogError("The target object tagged 'Player' does not have a Diver component!");
      }
    }
  }

  public void TakeDamage(int damageAmount) {
    health -= damageAmount;
    if (health <= 0) {
      Destroy(gameObject);
      Debug.Log("Octopus defeated!");
    }
  }
}
