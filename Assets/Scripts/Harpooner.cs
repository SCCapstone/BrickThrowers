using UnityEngine;
using System.Collections;

public class Harpooner : MonoBehaviour
{
    private Animator animator;
    public Transform attackPoint;
    public float attackRange = 1f;
    public LayerMask enemyLayer;
    public float knockbackForce = 10f;

    private CircleCollider2D attackCollider;

    void Start()
    {
        animator = GetComponent<Animator>();

        attackCollider = attackPoint.GetComponent<CircleCollider2D>();

        if (attackCollider == null)
        {
            attackCollider = attackPoint.gameObject.AddComponent<CircleCollider2D>();
            attackCollider.radius = attackRange;  // radius to the attackRange
            attackCollider.isTrigger = true;
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

        // After trigger attack animation, trigger "Attack" to transition back to idle/swimming.
        animator.SetTrigger("Attack");

        ApplyKnockbackToEnemies();
    }

    private void PlayAttackAnimation(string triggerName)
    {
        animator.SetTrigger(triggerName);
    }

    private void ApplyKnockbackToEnemies()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackCollider.transform.position, attackCollider.radius, enemyLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.CompareTag("Enemy"))
            {
                Rigidbody2D rb = enemy.GetComponent<Rigidbody2D>();

                if (rb != null)
                {
                    // this just calculates the direction away from the harpooner (attack point)
                    Vector2 knockbackDirection = enemy.transform.position - attackCollider.transform.position;
                    knockbackDirection.Normalize();

                    // this just applies a force in the opposite direction of the harpooner's attack making the enemies tag/layer go
                    rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
                }
            }
        }
    }
}
