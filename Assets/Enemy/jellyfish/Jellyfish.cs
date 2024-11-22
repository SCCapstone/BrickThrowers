// Jellyfish.cs
using UnityEngine;

public class Jellyfish : MonoBehaviour
{
    public float floatSpeed = 1.5f;               // Speed of jellyfish floating
    public float pulseInterval = 2f;              // Interval between random direction changes
    public float pulseDuration = 0.3f;            // Duration of each pulse movement
    public float stunDuration = 2f;               // Duration of diver's stun upon contact

    private Rigidbody2D rb;
    private Vector2 moveDirection;
    private float pulseTimer;
    private float pulseEndTimer;

    private bool isPulsing = false;               // Whether the jellyfish is currently pulsing

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ChangeDirection();
    }

    void Update()
    {
        // Pulse timer controls the interval between direction changes
        pulseTimer -= Time.deltaTime;

        if (!isPulsing && pulseTimer <= 0f)
        {
            StartPulse();
        }

        if (isPulsing)
        {
            pulseEndTimer -= Time.deltaTime;
            if (pulseEndTimer <= 0f)
            {
                StopPulse();
            }
        }

        // Move the jellyfish in the chosen direction
        MoveJellyfish();
    }

    void StartPulse()
    {
        isPulsing = true;
        pulseEndTimer = pulseDuration;
        pulseTimer = pulseInterval;
        ChangeDirection();
    }

    void StopPulse()
    {
        isPulsing = false;
    }

    void MoveJellyfish()
    {
        if (isPulsing)
        {
            rb.MovePosition(rb.position + moveDirection * floatSpeed * Time.deltaTime);
        }
    }

    void ChangeDirection()
    {
        // Randomly choose a new direction
        float angle = Random.Range(0f, 360f);
        moveDirection = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).normalized;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Check for collision with diver
        if (collision.CompareTag("Diver"))
        {
            collision.GetComponent<Diver>().Stun(stunDuration);
            Debug.Log("Diver stunned by jellyfish!");
        }
    }
}



//public class Diver : MonoBehaviour
//{
//    public int oxygenLevel = 100;
//    private bool isStunned = false;             // Whether the diver is currently stunned
//    private float stunTimer = 0f;

//    void Update()
//    {
//        if (isStunned)
//        {
//            stunTimer -= Time.deltaTime;
//            if (stunTimer <= 0f)
//            {
//                isStunned = false;
//                Debug.Log("Diver is no longer stunned.");
//            }
//        }
//    }

//    public void Stun(float duration)
//    {
//        isStunned = true;
//        stunTimer = duration;
//        Debug.Log("Diver is stunned!");
//    }

//    public void TakeOxygenDamage(int damage)
//    {
//        if (isStunned) return;  // Diver takes no action while stunned

//        oxygenLevel -= damage;
//        Debug.Log("Diver's oxygen level: " + oxygenLevel);

//        if (oxygenLevel <= 0)
//        {
//            Debug.Log("Diver has run out of oxygen!");
//            // Handle game over or level restart
//        }
//    }
//}
