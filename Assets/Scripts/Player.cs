using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Defines what a player should do.
/// </summary>
public class Player : Diver
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
    
  
    
    // Movement
    public float speed = 40f;
    public float verticalSpeed = 40f;
    public float fastSpeedMultiplier = 1.5f;
    public AudioSource swimsfx;
    private bool isSwimmingFast = false;

    public float baseSpeed = 40f;
    public float baseVerticalSpeed = 40f;

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

    // Stamina (oxygen for Players will be covered by the Diver's oxygenLevel)
    private float currentStamina;

    public Rigidbody2D rb;
    private Vector2 movement;

    //Xp and Currency
    public int currentXp, maxXp, currency, currentLevel;

    // Status effects
    [SerializeField]
    public bool isPoisoned = false;

    // Value gained from exploration
    [SerializeField]
    public int accumulatedValue = 0; // Total value gained during expeditions

    // Keybinds
    public KeyCode sprintKey = KeyCode.LeftShift;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = waterGravityScale;
        rb.drag = waterDrag;
        currentStamina = maxStamina;
    }

    public void Update()
    {
        base.Update();
        Movement();
        OxygenAndStamina();
        Sprint();
    }

    private void Movement() {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        float x = horizontalInput * speed;
        float y = verticalInput * verticalSpeed;
        if (Input.GetKey(KeyCode.LeftShift) && currentStamina > 0)
        {
            isSwimmingFast = true;
            x *= fastSpeedMultiplier;
        }
        else
        {
            isSwimmingFast = false;
        }
        rb.velocity = new Vector2(x, y);
        if (!swimsfx.isPlaying)
            swimsfx.Play();
    }

    /// <summary>
    /// Manages oxygen and stamina
    /// </summary>
    private void OxygenAndStamina()
    {
        float oxygenDepletion = oxygenDepletionRate * Time.deltaTime;

        if (currentStamina <= lowStaminaThreshold)
        {
            oxygenDepletion *= lowStaminaMultiplier;
        }

        oxygenLevel -= oxygenDepletion;
        oxygenLevel = Mathf.Clamp(oxygenLevel, 0, maxOxygen);

        if (isSwimmingFast)
        {
            currentStamina -= staminaDepletionRate * Time.deltaTime;
        }
        else
        {
            currentStamina += staminaRecoveryRate * Time.deltaTime;
        }

        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);

    }

    public float GetOxygenLevel() => oxygenLevel;
    public float GetStaminaLevel() => currentStamina;

    /// <summary>
    /// Allows the player to swim faster.
    /// </summary>
    public void Sprint()
    {
        if (Input.GetKey(sprintKey) && currentStamina > 0)
        {
            isSwimmingFast = true;
            // Use the base speeds multiplied by the sprint factor.
            speed = baseSpeed * fastSpeedMultiplier;
            verticalSpeed = baseVerticalSpeed * fastSpeedMultiplier;
        }
        else
        {
            isSwimmingFast = false;
            // Revert back to the normal speeds.
            speed = baseSpeed;
            verticalSpeed = baseVerticalSpeed;
        }
    }

    // Added because was in Lionfish.cs, and causing compile errors
    /// <summary>
    /// Applies a "Poison" effect to the player. Posion should cause increased oxygen decay, and halt
    /// stamina recovery.
    /// </summary>
    [ContextMenu("Apply Poison")]
    public void ApplyPoison() 
    {
        isPoisoned = true;
        oxygenDepletionRate *= 5;
        staminaRecoveryRate = 0;
    }

    /// <summary>
    /// Undos the "Poison" effect on the player. Restores oxygen decay and stamina recovery to normal.
    /// </summary>
    [ContextMenu("Relieve Poison")]
    public void RelievePoison()
    {
        isPoisoned = false;
        oxygenDepletionRate /= 5;
        staminaRecoveryRate = 1;
    }

    //// Added because was in PirateA.cs, and causing compile errors
    //public void TakeDamage(int val) {}

    //// Added because was in Shark.cs, and causing compile errors
    //// Also, does this not conflict with TakeDamage?
    //public void TakeOxygenDamage(int val) {}

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


    //Handling XP and Level up
    private void HandleExperienceChange(int newExperience)
    {
        currentXp += newExperience;
        if(currentXp >= maxXp)
        {
            LevelUp();
        }
    }

    //Focuses on Levelup and increases Xp cap
    private void LevelUp()
    {
      currentLevel++;
      currency += 10;
      maxXp += 100;
      currentXp = 0;
    }
    
    //Calls code
    //private void OnEnable()
    //{
    //    ExperienceManager.Instance.onExperienceChange += HandleExperienceChange;
    //}

    ////Stops calling code
    //private void OnDisable()
    //{
    //    ExperienceManager.Instance.onExperienceChange -= HandleExperienceChange;
    //}



    
}
