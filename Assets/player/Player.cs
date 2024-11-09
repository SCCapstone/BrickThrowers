using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Defines what a player should do.
/// </summary>
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

    // This class should never be used for player classes, but is the framework of classes.

    // Variables to allow water-like movement
    public float speed = 40f;
    public float verticalSpeed = 40f;

    public float waterDrag = 3f;
    public float waterGravityScale = 50f;


    public Rigidbody2D rb;
    private Vector2 movement;

    // Inventory
    private int inventorySize = 4;
    //private List<Item> inventory = new List<Item>();
    private int inventoryIndex = 0;

    // Items in Range
    private List<GameObject> itemsInRange = new List<GameObject>();

    /// <summary>
    /// Moves players according to particular player class parameters.
    /// Some classes move faster than others.
    /// </summary>
    public virtual void Movement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        rb.velocity = new Vector2(horizontalInput * speed, verticalInput * verticalSpeed);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Item":
                ItemInteract();
                break;
            case "Enemy":
                HealthDeplete();
                break;
            default:
                break;
        }
    }

    // Health Deplete
    public void HealthDeplete()
    {
        Debug.Log("Health Depleted");
    }

    // Item Interact
    public void ItemInteract()
    {
        Debug.Log("Item Interacted");
    }


}
