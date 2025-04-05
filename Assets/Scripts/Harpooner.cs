using UnityEngine;
using System.Collections;

public class Harpooner : MonoBehaviour
{
    private Animator animator;
    public Transform attackPoint;  // The point where the harpoon attack originates
    public float attackRange = 1f;  // The range around the attack point to detect enemies
    public LayerMask enemyLayer;  // Layer mask to detect enemies
    public float knockbackForce = 10f;  // Force applied to push enemies away

    private CircleCollider2D attackCollider;  // Reference to the CircleCollider2D component

    void Start()
    {
        animator = GetComponent<Animator>();

        // Ensure the attackPoint has a CircleCollider2D component attached
        attackCollider = attackPoint.GetComponent<CircleCollider2D>();

        // If the CircleCollider2D doesn't exist, add it dynamically
        if (attackCollider == null)
        {
            attackCollider = attackPoint.gameObject.AddComponent<CircleCollider2D>();
            attackCollider.radius = attackRange;  // Set the radius to the attackRange
            attackCollider.isTrigger = true;     // Ensure it doesn't physically interact with other objects
        }
    }

    void Update()
    {
        if (ClassSelectionData.SelectedClass == "Harpooner")
        {
            if (Input.GetMouseButtonDown(0))
            {
                HandleAttack();
            }
        }
    }

    private void HandleAttack()
    {
        Vector3 mousePos = Input.mousePosition;

        if (mousePos.x < Screen.width / 3f)
        {
            PlayAttackAnimation("PlayerHarpoon-Attack-L");
        }
        else if (mousePos.x > Screen.width * 2f / 3f)
        {
            PlayAttackAnimation("PlayerHarpoon-Attack-R");
        }
        else
        {
            PlayAttackAnimation("PlayerHarpoon-Attack-Up");
        }

        // After triggering the attack animation, trigger a generic "Attack" to transition back to idle/swimming.
        animator.SetTrigger("Attack");

        // Call the method to apply knockback on enemies within range
        ApplyKnockbackToEnemies();
    }

    private void PlayAttackAnimation(string triggerName)
    {
        animator.SetTrigger(triggerName);
    }

    private void ApplyKnockbackToEnemies()
    {
        // Detect all enemies within the attack range using the CircleCollider2D
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackCollider.transform.position, attackCollider.radius, enemyLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.CompareTag("Enemy")) // Check if the enemy is tagged "Enemy"
            {
                // Apply knockback force to the enemy
                Rigidbody2D rb = enemy.GetComponent<Rigidbody2D>();

                if (rb != null)
                {
                    // Calculate direction away from the harpooner (attack point)
                    Vector2 knockbackDirection = enemy.transform.position - attackCollider.transform.position;
                    knockbackDirection.Normalize();

                    // Apply a force in the opposite direction of the harpooner's attack
                    rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
                }
            }
        }
    }
}
