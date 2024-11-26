using UnityEngine;

public class Shark : MonoBehaviour
{
    public float patrolSpeed = 3f;               // Speed of shark patrol
    public float chargeSpeed = 7f;              // Speed of shark when charging
    public float chargeCooldown = 3f;           // Cooldown time between charges
    public float detectionRange = 10f;          // Detection range for spotting the diver
    public float chargeDuration = 2f;           // Duration of the charge
    public int oxygenDamage = 20;               // Amount of oxygen damage to the diver

    private Transform diver;
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
        GameObject diverObject = GameObject.FindGameObjectWithTag("Diver");
        if (diverObject != null)
        {
            diver = diverObject.transform;
        }
    }

    void Update()
    {
        if (rb == null) return;

        cooldownTimer -= Time.deltaTime;

        if (!isCharging && diver != null && cooldownTimer <= 0f && Vector2.Distance(transform.position, diver.position) <= detectionRange)
        {
            StartCharging();
        }

        if (isCharging)
        {
            chargeTimer -= Time.deltaTime;
            ChargeTowardsDiver();

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

    void ChargeTowardsDiver()
    {
        Vector2 chargeDirection = ((Vector2)diver.position - (Vector2)transform.position).normalized;
        rb.velocity = chargeDirection * chargeSpeed;
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
}
