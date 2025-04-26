using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class Harpooner : MonoBehaviour {
  [SerializeField] private Animator animator;
  public Transform attackPoint;
  public LayerMask enemyLayer;
  public float knockbackForce = 10f;

  // Actions
  public PlayerInputActions playerControls;
  private InputAction attack;

  #region Setup Functions
  private void Awake() {
    playerControls = new PlayerInputActions();
    attack = playerControls.Player.Attack;
  }
  private void OnEnable() {

    attack.Enable();
    attack.performed += Attack;
  }
  private void OnDisable() {
    attack.performed -= Attack;
    attack.Disable();
  }
  #endregion
  #region Attack Logic
  public void Attack(InputAction.CallbackContext context) {

    // inside your Update or Attack()…
    Vector3 mouseScreen = Input.mousePosition;
    // 1) get direction
    Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    Vector2 dir = mouseWorld - transform.position;
    dir.Normalize();

    if (dir.y > Mathf.Abs(dir.x)) {
      // up: y is dominant and positive
      Debug.Log("Attack up");
      animator.SetTrigger("Attack-U");
    } else if (dir.x > Mathf.Abs(dir.y)) {
      // right: x positive and dominant
      Debug.Log("Attack right");
      animator.SetTrigger("Attack-R");
    } else if (-dir.x > Mathf.Abs(dir.y)) {
      // left: x negative and |x| dominant
      Debug.Log("Attack left");
      animator.SetTrigger("Attack-L");
    } else {
      // dir.y is negative (downwards)—ignore or default
      Debug.Log("at least you are in the loop, eh?");
    }

    OnAttackAnimationEnd();

  }

  #endregion
  #region Animation
  public void OnAttackAnimationEnd() {
    animator.SetTrigger("Attack");
  }
  #endregion

  #region Old Code
  /*
     private Animator animator;
  public Transform attackPoint;
  public float attackRange = 1f;
  public LayerMask enemyLayer;
  public float knockbackForce = 10f;

  private CircleCollider2D attackCollider;

  void Start() {
    animator = GetComponent<Animator>();

    attackCollider = attackPoint.GetComponent<CircleCollider2D>();

    if (attackCollider == null) {
      attackCollider = attackPoint.gameObject.AddComponent<CircleCollider2D>();
      attackCollider.radius = attackRange;  // radius to the attackRange
      attackCollider.isTrigger = true;
    }
  }

  void Update() {
    if (ClassSelectionData.SelectedClass == "Harpooner") {
      if (Input.GetMouseButtonDown(0)) {
        HandleAttack();
      }
    }
  }

  private void HandleAttack() {
    Vector3 mousePos = Input.mousePosition;

    if (mousePos.x < Screen.width / 3f) {
      PlayAttackAnimation("Attack-L");
    } else if (mousePos.x > Screen.width * 2f / 3f) {
      PlayAttackAnimation("Attack-R");
    } else {
      PlayAttackAnimation("Attack-Up");
    }

    // After trigger attack animation, trigger "Attack" to transition back to idle/swimming.
    animator.SetTrigger("Attack");

    ApplyKnockbackToEnemies();
  }

  private void PlayAttackAnimation(string triggerName) {
    animator.SetTrigger(triggerName);
  }


   */
  #endregion
}
