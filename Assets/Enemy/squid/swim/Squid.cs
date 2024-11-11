// Squid.cs
using UnityEngine;

public class Squid : MonoBehaviour
{
    public float ambushSpeed = 7f;              // Speed of squid during ambush
    public float retreatSpeed = 10f;            // Speed of squid when retreating
    public float ambushRange = 8f;              // Range at which squid detects diver and begins ambush
    public float inkBlindDuration = 3f;         // Duration for which the diver is blinded
    public float retreatDuration = 1.5f;        // Duration of squid's retreat after ambush

    private Transform diver;
    private Vector2 retreatDirection;
    private bool isAmbushing = false;
    private bool isRetreating = false;
    private float retreatTimer = 0f;

    private Rigidbody2D rb;

    void Start()
    {
        diver = GameObject.FindGameObjectWithTag("Diver").transform;
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (isAmbushing)
        {
            AmbushDiver();
        }
        else if (isRetreating)
        {
            retreatTimer -= Time.deltaTime;
            if (retreatTimer <= 0f)
            {
                isRetreating = false;
            }
            Retreat();
        }
        else
        {
            DetectDiverAndAmbush();
        }
    }

    void DetectDiverAndAmbush()
    {
        // Start ambush if diver is within ambush range
        if (Vector2.Distance(transform.position, diver.position) <= ambushRange)
        {
            isAmbushing = true;
        }
    }

    void AmbushDiver()
    {
        // Move towards the diver quickly
        Vector2 directionToDiver = (diver.position - transform.position).normalized;
        rb.MovePosition(rb.position + directionToDiver * ambushSpeed * Time.deltaTime);

        // If close enough to diver, release ink and retreat
        if (Vector2.Distance(transform.position, diver.position) <= 1f)
        {
            ReleaseInk();
            StartRetreat(directionToDiver);
        }
    }

    void StartRetreat(Vector2 directionToDiver)
    {
        isAmbushing = false;
        isRetreating = true;
        retreatDirection = -directionToDiver; // Retreat in the opposite direction
        retreatTimer = retreatDuration;
    }

    void Retreat()
    {
        rb.MovePosition(rb.position + retreatDirection * retreatSpeed * Time.deltaTime);
    }

    void ReleaseInk()
    {
        Debug.Log("Squid releases ink, blinding the diver!");
        diver.GetComponent<Diver>().Blind(inkBlindDuration);
    }
}

// Diver.cs (Updated to handle blinding effect)
using UnityEngine;

public class Diver : MonoBehaviour
{
    public int oxygenLevel = 100;
    private bool isBlinded = false;             // Whether the diver is currently blinded
    private float blindTimer = 0f;

    void Update()
    {
        if (isBlinded)
        {
            blindTimer -= Time.deltaTime;
            if (blindTimer <= 0f)
            {
                isBlinded = false;
                Debug.Log("Diver's vision restored.");
                // Restore diver's vision effect here
            }
        }
    }

    public void Blind(float duration)
    {
        isBlinded = true;
        blindTimer = duration;
        Debug.Log("Diver is blinded by squid ink!");
        // Trigger vision-obscuring effect here, like fading screen or overlaying dark filter
    }

    public void TakeOxygenDamage(int damage)
    {
        if (isBlinded) return;  // Optional: Divers could take reduced or no actions when blinded

        oxygenLevel -= damage;
        Debug.Log("Diver's oxygen level: " + oxygenLevel);

        if (oxygenLevel <= 0)
        {
            Debug.Log("Diver has run out of oxygen!");
            // Handle game over or level restart
        }
    }
}
