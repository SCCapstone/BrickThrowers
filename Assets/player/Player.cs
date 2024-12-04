using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Player : MonoBehaviour
{
    /*
     * A player class should be a parent class where all the player related classes should inherit from.
     * As a parent class, it should have all the common properties and methods that all the player related classes should have.
     * Players should have movement, health, and other properties that are common to all players.
     * The setting takes place in water, so movement nautrually should be slow to simulate the resistance of water.
     * This game is also in a 2D environment, so movement should be restricted to the x and y axis.
     * Additionally, players should have inventory, and interaction with inventory items and other objects in the game.
     * Actual in-game player classes should inherit from this class and implement their own unique properties and methods.
     * Players all emit some form of light, so they should have a light source attached to them.
     */
    
    [SerializeField]
    // Movement
    public float speed = 40f;
    public float verticalSpeed = 40f;
    public float fastSpeedMultiplier = 1.5f;
    private bool isSwimmingFast = false;
    
    // Water Physics
    public float waterDrag = 3f;
    public float waterGravityScale = 50f;

    // Oxygen and Stamina
    public float maxOxygen = 100f;
    public float oxygenDepletionRate = 1f;
    public float lowStaminaMultiplier = 5f;

    public float maxStamina = 20f;
    public float staminaDepletionRate = 2f;
    public float staminaRecoveryRate = 1f;
    public float lowStaminaThreshold = 5f;

    private float currentOxygen;
    private float currentStamina;

    public Rigidbody2D rb;
    private Vector2 movement;

    // Inventory
    //private int inventorySize = 4;
    //private List<Item> inventory = new List<Item>();
    //private int inventoryIndex = 0;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = waterGravityScale;
        rb.drag = waterDrag;
        currentOxygen = maxOxygen;
        currentStamina = maxStamina;
    }

    void Update() {
        Movement();
        OxygenAndStamina();
    }

    private void Movement() {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        float x = horizontalInput * speed;
        float y = verticalInput * verticalSpeed;

        if (Input.GetKey(KeyCode.LeftShift) && currentStamina > 0) {
            isSwimmingFast = true;
            x *= fastSpeedMultiplier;
        } else {
            isSwimmingFast = false;
        }
        rb.velocity = new Vector2(x,y);
    }

    private void OxygenAndStamina() {
        float oxygenDepletion = oxygenDepletionRate * Time.deltaTime;

        if (currentStamina <= lowStaminaThreshold) {
            oxygenDepletion *= lowStaminaMultiplier;
        }

        currentOxygen -= oxygenDepletion;
        currentOxygen = Mathf.Clamp(currentOxygen,0,maxOxygen);

        if (isSwimmingFast) {
            currentStamina -= staminaDepletionRate * Time.deltaTime;
        } else {
            currentStamina += staminaRecoveryRate * Time.deltaTime;
        }

        currentStamina = Mathf.Clamp(currentStamina,0,maxStamina);
    }

    public float GetOxygenLevel() => currentOxygen;
    public float GetStaminaLevel() => currentStamina;

    // Added because was in Lionfish.cs, and causing compile errors
    public void ApplyPoison() {}

    // Added because was in PirateA.cs, and causing compile errors
    public void TakeDamage(int val) {}

    // Added because was in Shark.cs, and causing compile errors
    // Also, does this not conflict with TakeDamage?
    public void TakeOxygenDamage(int val) {}

    // Added because was in PirateB.cs, and causing compile errors
    public bool HasArtifact() {return true;}
    public void RemoveArtifact() {}

    // Added becasue was in Squid.cs, and casuing compile errors
    public void Blind(float duration) {}

    // Added because was in Jellyfish.cs, and causing compile errors
    public void Stun(float duration) {}

    // Added becasue was in Octopus.cs, and causing compile errors.
    public void SuppressLight(bool val) {}
    public void SuppressMovement(bool val) {}
    public int GetNearbyDivers() {return 0;}
    
}
