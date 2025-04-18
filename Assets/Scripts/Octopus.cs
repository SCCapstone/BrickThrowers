using System.Collections;
using UnityEngine;

public class Octopus : MonoBehaviour
{
    public float detectionRange = 3f; // Range to detect the player
    public float latchDuration = 5f; // Default time to free from latch
    public int health = 60; // Health of the octopus
    public float moveSpeed = 15f; // Movement speed when roaming

    private Transform targetPlayer; // Player being targeted
    private Rigidbody2D rb;
    private bool isLatched = false; // Whether the octopus is latched onto the player
    private float currentLatchTime; // Remaining time for latch release
    private int decayMultiplier = 1; // Multiplier for latch decay

    private bool isRoaming = false;
    [SerializeField] private float roamDuration = 2f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Roam();
    }

    void Update()
    {
        //if (!isLatched)
        //{
        //    // Random roaming behavior
            
        //}
        if (!isRoaming)
        {
            StartCoroutine(Roam());
        }
    }

    #region Old Latch Logic
    //void DetectPlayer()
    //{
    //    GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
    //    if (playerObject != null)
    //    {
    //        float distance = Vector2.Distance(transform.position, playerObject.transform.position);
    //        if (distance <= detectionRange)
    //        {
    //            targetPlayer = playerObject.transform;
    //            Latch(targetPlayer.GetComponent<Player>());
    //        }
    //    }
    //}

    //void Latch(Player player)
    //{
    //    if (player == null) return;

    //    isLatched = true;
    //    currentLatchTime = latchDuration;
    //    player.SuppressMovement(true);
    //    player.SuppressLight(true);
    //    Debug.Log("Octopus latched onto the player!");
    //}

    //void LatchOntoPlayer()
    //{
    //    if (targetPlayer == null) return;

    //    Player player = targetPlayer.GetComponent<Player>();
    //    if (player == null) return;

    //    currentLatchTime -= Time.deltaTime;

    //    // Simulate assistance from other divers
    //    int assistingDivers = player.GetNearbyDivers();
    //    if (assistingDivers > 0)
    //    {
    //        currentLatchTime -= assistingDivers * Time.deltaTime;
    //    }

    //    if (currentLatchTime <= 0f)
    //    {
    //        Release(player);
    //    }
    //}

    //void Release(Player player)
    //{
    //    isLatched = false;
    //    targetPlayer = null;
    //    player.SuppressMovement(false);
    //    player.SuppressLight(false);
    //    Debug.Log("Player freed from the octopus!");
    //}

    #endregion
    #region Roam Logic
    IEnumerator Roam()
    {
        isRoaming = true;
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        rb.velocity = randomDirection * moveSpeed;
        yield return new WaitForSeconds(roamDuration);
        isRoaming = false;

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
    #endregion
    #region Latch Logic
    // Called when the octopus collides with the player
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();

            if (player != null)
            {
                // Latching logic for the player
                StartCoroutine(LatchOntoPlayer(player, collision.gameObject.transform));
            }
        }
    }

    private void Latch(Player player, Transform playerTransform)
    {
        player.SuppressMovement(true);
        player.SuppressLight(true);
        Debug.Log("Octopus latch onto player.");
        // Have the octopus game object cling onto the player
        // while the latch timer is running
        this.transform.SetParent(playerTransform);
        rb.constraints = RigidbodyConstraints2D.FreezePositionX;
        rb.drag = 1f;
        rb.gravityScale = 1f;
    }

    /*
     * Octopus grabs player.d
     * Player has no visibilty.
     * Player has no movement.
     * Last as long as the timer is running.
     * When the timer finishes, return the player to normal.
     * Have the octopus roam away from the player.
     */
    IEnumerator LatchOntoPlayer(Player player, Transform playerTransform)
    {
        isLatched = true;
        currentLatchTime = latchDuration;
        Latch(player, playerTransform);
        while (currentLatchTime > 0f)
        {
            currentLatchTime -= Time.deltaTime * decayMultiplier;
            yield return null;
        }
        Release(player);
        Vector2 direction = (transform.position - player.transform.position).normalized;
        rb.velocity = direction * moveSpeed;
    }

    void Release(Player player)
    {
        isLatched = false;
        player.SuppressMovement(false);
        player.SuppressLight(false);
        this.transform.SetParent(null);
        Debug.Log("Player freed from the octopus!");
        rb.constraints = RigidbodyConstraints2D.None;
        rb.drag = 0f; // Reset drag to normal
        rb.gravityScale = 0f;
    }
    #endregion
}
