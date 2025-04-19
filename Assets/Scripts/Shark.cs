using System.Collections;
using UnityEngine;

public class Shark : MonoBehaviour
{
    public float patrolSpeed = 3f; // Speed of shark patrol
    public float chargeSpeed = 7f; // Speed of shark when charging
    public float chargeCooldown = 3f; // Cooldown time between charges
    public float detectionRange = 10f; // Detection range for spotting the player
    public float chargeDuration = 10f; // Duration of the charge
    public int oxygenDamage = 20; // Amount of oxygen damage to the player

    private Transform targetPlayer;
    private Rigidbody2D rb;

    private float chargeTimer = 0f;
    private float cooldownTimer = 0f;
    public float roamDuration = 2f;
    private Vector2 patrolDirection;
    public bool isCharging = false;
    public bool isPatrolling = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("No Rigidbody2D attached to the Shark GameObject.");
            return;
        }

        patrolDirection = GetRandomDirection(); // Initialize patrol direction
    }

    #region Old Logic
    void Update()
    {
        if (!isCharging)
        {
            if (!isPatrolling)
            {
                StartCoroutine(Patrol());
            }
        }
    }

    IEnumerator Patrol()
    {
        isPatrolling = true;
        // Move in a set direction

        //Debug.Log($"{rb.velocity}");

        //// Randomly change direction over time
        //if (Random.Range(0f, 1f) < 0.01f)
        //{
        //    patrolDirection = GetRandomDirection();
        //}

        //rb.velocity = patrolDirection * patrolSpeed;
        //yield return new WaitForSeconds(roamDuration); // Adjust patrol frequency

        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        rb.velocity = randomDirection * patrolSpeed;
        yield return new WaitForSeconds(roamDuration);

        isPatrolling = false;
    }

    Vector2 GetRandomDirection()
    {
        float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).normalized;
    }
    #endregion
    #region Charge Logic
    void Charge()
    {
        // Setup parameters for charge
        isCharging = true;
        chargeTimer = chargeDuration;
        cooldownTimer = chargeCooldown;

        // Charge
        StartCoroutine(ChargePlayer());
    }

    IEnumerator ChargePlayer()
    {
        Vector2 chargeDirection = (
            (Vector2)targetPlayer.position - (Vector2)transform.position
        ).normalized;
        rb.velocity = chargeDirection * chargeSpeed;
        yield return new WaitForSeconds(chargeDuration);
        // Stop the shark after the charge
        StopCharging();

        // Enter cooldown
        StartCoroutine(ChargeCooldown());
    }

    void StopCharging()
    {
        isCharging = false;
        rb.velocity = Vector2.zero; // Stop the shark after charge
    }

    IEnumerator ChargeCooldown()
    {
        yield return new WaitForSeconds(cooldownTimer);
        cooldownTimer = 0f;
    }
    #endregion
    #region Collision
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Player player = collision.collider.GetComponent<Player>();
            if (player != null)
            {
                player.TakeOxygenDamage(oxygenDamage);
            }
        }
    }
    #endregion
    #region Triggers
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            targetPlayer = collision.transform;
            if (!isCharging && cooldownTimer <= 0f)
            {
                Charge();
            }
        }
    }
    #endregion
}
