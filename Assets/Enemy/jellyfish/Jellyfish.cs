using UnityEngine;

public class Jellyfish : MonoBehaviour
{
    public float floatSpeed = 1.5f;               // Speed of jellyfish floating
    public float directionChangeInterval = 2f;   // Interval for changing direction
    public float stunDuration = 2f;              // Duration of player's stun upon contact

    private Rigidbody2D rb;
    private Vector2 moveDirection;
    private float directionChangeTimer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("No Rigidbody2D attached to the Jellyfish GameObject.");
            return;
        }

        ChangeDirection(); // Set initial movement direction
        directionChangeTimer = directionChangeInterval;
    }

    void Update()
    {
        if (rb == null) return;

        // Timer for direction change
        directionChangeTimer -= Time.deltaTime;
        if (directionChangeTimer <= 0f)
        {
            ChangeDirection();
            directionChangeTimer = directionChangeInterval;
        }

        // Move the jellyfish
        rb.velocity = moveDirection * floatSpeed;
    }

    void ChangeDirection()
    {
        // Choose a random direction
        float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        moveDirection = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).normalized;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            if (player != null)
            {
                player.Stun(stunDuration);
                Debug.Log("Player stunned by Jellyfish!");
            }
            else
            {
                Debug.LogError("The collided object tagged 'Player' does not have a Player component!");
            }
        }
    }
}
