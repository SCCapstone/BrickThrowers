// Shark.cs
using UnityEngine;

public class Shark : MonoBehaviour
{
    public float speed = 5f;
    public float chargeCooldown = 3f;
    public float detectionRange = 10f;
    public float chargeDuration = 2f;
    public int oxygenDamage = 20;

    private Transform diver;
    private bool isCharging = false;
    private float chargeTimer = 0f;
    private float cooldownTimer = 0f;
    private Vector2 chargeDirection;

    private Rigidbody2D rb;

    void Start()
    {
        diver = GameObject.FindGameObjectWithTag("Diver").transform;
        rb = GetComponent<Rigidbody2D>(); // Assumes a Rigidbody2D is attached to the shark GameObject
        rb.isKinematic = true; // Set to kinematic for direct position control
    }

    void Update()
    {
        cooldownTimer -= Time.deltaTime;

        if (!isCharging && cooldownTimer <= 0f && Vector2.Distance(transform.position, diver.position) <= detectionRange)
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
    }

    void StartCharging()
    {
        isCharging = true;
        chargeDirection = (diver.position - transform.position).normalized;
        chargeTimer = chargeDuration;
    }

    void ChargeTowardsDiver()
    {
        Vector2 newPosition = (Vector2)transform.position + chargeDirection * speed * Time.deltaTime;
        rb.MovePosition(newPosition);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, chargeDirection, speed * Time.deltaTime);
        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Diver"))
            {
                hit.collider.GetComponent<Diver>().TakeOxygenDamage(oxygenDamage);
                StopCharging();
            }
            else if (hit.collider.CompareTag("Destructible"))
            {
                Destroy(hit.collider.gameObject);
            }
        }
    }

    void StopCharging()
    {
        isCharging = false;
        cooldownTimer = chargeCooldown;
    }
}
