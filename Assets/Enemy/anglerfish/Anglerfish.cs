using UnityEngine;

public class Anglerfish : MonoBehaviour
{
    public float swimSpeed = 2f;                 // Speed of anglerfish movement
    public float directionChangeInterval = 5f;  // Time between direction changes
    public Light anglerLight;                   // The anglerfish's light
    public float lightIntensity = 3f;           // Maximum light intensity
    public float flickerFrequency = 0.1f;       // Frequency of light flicker

    private Rigidbody2D rb;
    private Vector2 swimDirection;
    private float directionChangeTimer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("No Rigidbody2D attached to the Anglerfish GameObject.");
            return;
        }

        if (anglerLight != null)
        {
            InvokeRepeating(nameof(FlickerLight), 0f, flickerFrequency); // Start light flickering
        }

        swimDirection = GetRandomDirection();
        directionChangeTimer = directionChangeInterval;
    }

    void Update()
    {
        if (rb == null) return;

        // Timer for changing swim direction
        directionChangeTimer -= Time.deltaTime;
        if (directionChangeTimer <= 0f)
        {
            swimDirection = GetRandomDirection();
            directionChangeTimer = directionChangeInterval;
        }

        // Move the anglerfish
        rb.velocity = swimDirection * swimSpeed;
    }

    void FlickerLight()
    {
        if (anglerLight != null)
        {
            anglerLight.intensity = Random.Range(0.5f, lightIntensity); // Randomize light intensity
        }
    }

    Vector2 GetRandomDirection()
    {
        float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).normalized;
    }
}
