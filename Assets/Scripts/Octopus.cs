using UnityEngine;

public class Octopus : MonoBehaviour
{
    public float detectionRange = 3f;            // Range to detect the player
    public float latchDuration = 5f;            // Default time to free from latch
    public int health = 60;                     // Health of the octopus
    public float moveSpeed = 2f;                // Movement speed when roaming

    private Transform targetPlayer;             // Player being targeted
    private Rigidbody2D rb;
    private bool isLatched = false;             // Whether the octopus is latched onto the player
    private float currentLatchTime;             // Remaining time for latch release

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!isLatched)
        {
            // Detect the player
            DetectPlayer();

            // Random roaming behavior
            Roam();
        }
        else
        {
            // Handle latch logic
            if (targetPlayer != null)
            {
                LatchOntoPlayer();
            }
        }
    }

    void DetectPlayer()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            float distance = Vector2.Distance(transform.position, playerObject.transform.position);
            if (distance <= detectionRange)
            {
                targetPlayer = playerObject.transform;
                Latch(targetPlayer.GetComponent<Player>());
            }
        }
    }

    void Latch(Player player)
    {
        if (player == null) return;

        isLatched = true;
        currentLatchTime = latchDuration;
        player.SuppressMovement(true);
        player.SuppressLight(true);
        Debug.Log("Octopus latched onto the player!");
    }

    void LatchOntoPlayer()
    {
        if (targetPlayer == null) return;

        Player player = targetPlayer.GetComponent<Player>();
        if (player == null) return;

        currentLatchTime -= Time.deltaTime;

        // Simulate assistance from other divers
        int assistingDivers = player.GetNearbyDivers();
        if (assistingDivers > 0)
        {
            currentLatchTime -= assistingDivers * Time.deltaTime;
        }

        if (currentLatchTime <= 0f)
        {
            Release(player);
        }
    }

    void Release(Player player)
    {
        isLatched = false;
        targetPlayer = null;
        player.SuppressMovement(false);
        player.SuppressLight(false);
        Debug.Log("Player freed from the octopus!");
    }

    void Roam()
    {
        // Add simple random roaming movement
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        rb.velocity = randomDirection * moveSpeed;
    }

    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;
        if (health <= 0)
        {
            Destroy(gameObject);
            Debug.Log("Octopus defeated!");
        }
    }
}
