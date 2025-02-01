using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    /*
        * The Enemy Class is a parent class for all enemies in the game.
        * This class provides the basic components and functionality
        * common to all enemies, including movement, health, speed, and attack.
        * Enemies operate on the 2D XY plane and can focus on the player.
        * Feature: Added health reduction and death mechanics.
    */

    [Header("Enemy Attributes")]
    [SerializeField] private float speed = 6f; // Speed of enemy movement
    [SerializeField] private int damage = 4; // Damage dealt to the player
    [SerializeField] private int maxHealth = 6; // Max health of the enemy

    private int currentHealth; // Current health of the enemy
    private Rigidbody2D rigidbody2D; // Rigidbody for movement
    private Vector2 movement; // Movement direction

    void Awake()
    {
        // Initialize health and cache Rigidbody2D
        currentHealth = maxHealth;
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Move towards a target (e.g., player)
    public virtual void Move(Vector2 targetPosition)
    {
        Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
        movement = direction * speed;

        // Apply movement to the Rigidbody
        if (rigidbody2D != null)
        {
            rigidbody2D.velocity = movement;
        }
    }

    // Reduce health when taking damage
    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;

        // Check for death
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Handle enemy death
    protected virtual void Die()
    {
        // Destroy the enemy GameObject
        Destroy(gameObject);
    }

    // Getter for damage value (to deal damage to the player)
    public int GetDamage()
    {
        return damage;
    }

    // Debug to test health system
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }
}
