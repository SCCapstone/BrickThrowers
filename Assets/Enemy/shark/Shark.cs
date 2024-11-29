using UnityEngine;

public class Shark : MonoBehaviour
{
    public float patrolSpeed = 3f;               // Speed of shark patrol
    public float chargeSpeed = 7f;              // Speed of shark when charging
    public float chargeCooldown = 3f;           // Cooldown time between charges
    public float detectionRange = 10f;          // Detection range for spotting the player
    public float chargeDuration = 2f;           // Duration of the charge
    public int oxygenDamage = 20;               // Amount of oxygen damage to the player

    private Transform targetPlayer;
    private Rigidbody2D rb;
    private bool isCharging = false;
    private float chargeTimer = 0f;
    private float cooldownTimer = 0f;
    private Vector2 patrolDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("No Rigidbody2D attached to the Shark GameObject.");
            return;
        }

        patrolDirection = GetRandomDirection(); // Initialize patrol direction

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            targetPlayer = playerObject.transform;
        }
        else
        {
            Debug.LogError("No GameObject with tag 'Player' found in the scene.");
        }
    }

    void Update()
    {
        if (rb == null) return;

        cooldownTimer -= Time.deltaTime;

        if (!isCharging && targetPlayer != null && cooldownTimer <= 0f && Vector2.Distance(transform.position, targetPlayer.position) <= detectionRange)
        {
            StartCharging();
        }

        if (isCharging)
        {
            chargeTimer -= Time.deltaTime;
            ChargeTowardsPlayer();

            if (chargeTimer <= 0f)
            {
                StopCharging();
            }
        }
        else
        {
            Patrol();
        }
    }

    void Patrol()
    {
        // Move in a set direction
        rb.velocity = patrolDirection * patrolSpeed;

        // Randomly change direction over time
        if (Random.Range(0f, 1f) < 0.01f)
        {
            patrolDirection = GetRandomDirection();
        }
    }

    void StartCharging()
    {
        isCharging = true;
        chargeTimer = chargeDuration;
        cooldownTimer = chargeCooldown;
    }

    void ChargeTowardsPlayer()
    {
        if (targetPlayer == null) return;

        Vector2 chargeDirection = ((Vector2)targetPlayer.position - (Vector2)transform.position).normalized;
        rb.velocity = chargeDirection * chargeSpeed;

        // Optional: Check for direct collisions during charge using raycast or triggers
    }

    void StopCharging()
    {
        isCharging = false;
        rb.velocity = Vector2.zero; // Stop the shark after charge
    }

    Vector2 GetRandomDirection()
    {
        float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).normalized;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Player player = collision.collider.GetComponent<Player>();
            if (player != null)
            {
                player.TakeOxygenDamage(oxygenDamage);
                Debug.Log("Shark dealt damage to the player!");
            }
        }
    }
}
