using System.Collections;
using System.Collections.Generic;
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

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = waterGravityScale;
        rb.drag = waterDrag;
    }

    void Update() {
        Movement();
    }

    public void Movement() {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        rb.velocity = new Vector2(horizontalInput * speed, verticalInput * verticalSpeed);
    }
}
