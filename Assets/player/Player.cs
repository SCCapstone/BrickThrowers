using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Defines what a player should do.
/// </summary>
public abstract class Player : Diver
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

    // Health
    public int health = 100;

    // Inventory
    public InventoryObject inventory;


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
        // Action to collect item

    }

    public void Movement()
    {
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
    }

    /// <summary>
    /// Actions when colliders stay within each other.
    /// </summary>
    /// <param name="collision"></param>
    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Item"))
        {
            GroundItem item = collision.gameObject.GetComponent<GroundItem>();
            bool add_good = inventory.AddItem(new Item(item.item), 1);
            if (add_good)
                Destroy(collision.gameObject);

        }
    }

    // Health Deplete
    public virtual void HealthDeplete()
    {
        // Deplete health so long as player is collion with an enemy
        // On 0 health, player dies, the player GameObject destroys itself
        health -= 25;
        if (health <= 0)
        {
            Debug.Log("health = " + health);
            Destroy(gameObject);
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

    // Activities when you quit application
    private void OnApplicationQuit()
    {
        inventory.Container.Items.Clear();
    }

    public void DropItem()
    {
        // Remove an item to return it to this function, then drop the item in the scene world.
        //Randomize the location of the item in the scene world where dropped relative to the player.
        // It should be in the same radius of the player's item trigger child GameObject collision box.
        // The item is not a child of the player GameObject. It is a separate GameObject in the scene world.

        Item item = inventory.RemoveItem();
        if (item == null) {
            return;
        }
        Vector3 playerPosition = transform.position;
        Vector3 randomPosition = new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f), 0);
        randomPosition.Normalize();

    }
}

// Test save and load items
//public void SaveAndLoadTest()
//{
//    if (Input.GetKeyDown(KeyCode.Space))
//    {
//        Debug.Log("I entered the save place");
//        inventory.Save();
//    }
//    if (Input.GetKeyDown(KeyCode.L))
//    {
//        Debug.Log("I entered the load place");
//        inventory.Load();
//    }
//}

//public void OnCollisionEnter2D(Collision2D collision)
//{
//    switch (collision.gameObject.tag)
//    {
//        case "Item":
//            ItemInteract();
//            break;
//        case "Enemy":
//            HealthDeplete();
//            break;
//        default:
//            break;
//    }
//}