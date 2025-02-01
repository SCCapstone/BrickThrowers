using UnityEngine;

public class PirateB : MonoBehaviour
{
    public float patrolSpeed = 2f;          // Movement speed during patrol
    public float detectionRange = 5f;      // Range to detect player
    public float artifactStealRange = 1.5f;
    public int health = 40;

    private Rigidbody2D rb;
    private Vector2 patrolDirection;
    private float changeDirectionTimer = 3f;
    private Transform detectedPlayer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ChangeDirection();
    }

    void Update()
    {
        changeDirectionTimer -= Time.deltaTime;

        // Patrol if no player detected
        if (detectedPlayer == null)
        {
            Patrol();
            DetectPlayer();
        }
        else
        {
            ChasePlayer();
        }

        if (changeDirectionTimer <= 0f)
        {
            ChangeDirection();
            changeDirectionTimer = Random.Range(2f, 4f);
        }
    }

    void Patrol()
    {
        rb.velocity = patrolDirection * patrolSpeed;
    }

    void ChangeDirection()
    {
        float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        patrolDirection = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).normalized;
    }

    void DetectPlayer()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            float distance = Vector2.Distance(transform.position, playerObject.transform.position);
            if (distance <= detectionRange)
            {
                detectedPlayer = playerObject.transform;
            }
        }
    }

    void ChasePlayer()
    {
        if (detectedPlayer != null)
        {
            Vector2 direction = ((Vector2)detectedPlayer.position - (Vector2)transform.position).normalized;
            rb.velocity = direction * patrolSpeed;

            if (Vector2.Distance(transform.position, detectedPlayer.position) <= artifactStealRange)
            {
                StealArtifact(detectedPlayer.gameObject);
            }
        }
    }

    void StealArtifact(GameObject player)
    {
        Player playerComponent = player.GetComponent<Player>();
        if (playerComponent != null && playerComponent.HasArtifact())
        {
            playerComponent.RemoveArtifact();
            Debug.Log("PirateB stole an artifact!");
        }
    }

    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
