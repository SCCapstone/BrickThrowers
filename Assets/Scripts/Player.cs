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
    
    [SerializeField]
    // Movement
    public float speed = 40f;
    public float verticalSpeed = 40f;
    public float fastSpeedMultiplier = 1.5f;
    public AudioSource swimsfx;
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
    private bool nearItem; // If the player is near an item, this is true.
    public List<GameObject> nearestItems = new List<GameObject>();

    // Item Interaction
    public KeyCode pickUpItemKey = KeyCode.E;
    public KeyCode dropItemKey = KeyCode.Q;

    //Xp and Currency
    public int currentXp, maxXp, currency, currentLevel;



    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = waterGravityScale;
        rb.drag = waterDrag;
        currentOxygen = maxOxygen;
        currentStamina = maxStamina;
    }

    public void Update()
    {
        base.Update();
        Movement();
        OxygenAndStamina();
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

    public void AddItem(GameObject _gameObj)
    {
        
    }

    /// <summary>
    /// If the player is near an item, switch to true.
    /// </summary>
    /// <param name="collision"></param>
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Item"))
        {
            nearestItems.Add(collision.gameObject);
            nearItem = true;
        }
    }

    /// <summary>
    /// If the player has left item vicinity, this is false.
    /// </summary>
    /// <param name="collision"></param>
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Item"))
        {
            // find the gameObject within the nearest items list, and then remove it.
            nearestItems.Remove(collision.gameObject);
            nearItem = false;
        }
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

        currentOxygen -= oxygenDepletion;
        currentOxygen = Mathf.Clamp(currentOxygen, 0, maxOxygen);

        if (isSwimmingFast)
        {
            currentStamina -= staminaDepletionRate * Time.deltaTime;
        }
        else
        {
            currentStamina += staminaRecoveryRate * Time.deltaTime;
        }

        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
        // Item Interact

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

    // Activities when you quit application
    private void OnApplicationQuit()
    {
        
        
    }

    /// <summary>
    /// Not ideal, but the current implementation is disabling the item when adding it to inventory, and changing position and enable when item is added.
    /// </summary>
    public void DropItem()
    {
        

    }

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
