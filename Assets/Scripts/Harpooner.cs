// Copyright 2025 Brick Throwers
// // Harpooner.cs - Handles the harpooner class's attack logic and animations.
using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System;

public class Harpooner : MonoBehaviour {
  // Enum
  public enum Direction {
    up,
    right,
    left
  }

  // Inspector Variables
  [SerializeField] private Animator animator;
  public float knockbackForce = 10f;
  private LayerMask enemyLayerMask;
  [SerializeField] private RuntimeAnimatorController harpoonerAnimator;
  [SerializeField] private Animator playerSpriteAnimator;

  // Actions
  public PlayerInputActions playerControls;
  private InputAction attack;
  public static event Action<Direction> onAttack;

  // Attack zones
  [SerializeField] private GameObject attackZone;

  #region Setup Functions
  private void Awake() {
    playerControls = new PlayerInputActions();
    attack = playerControls.Player.Attack;
  }
  private void OnEnable() {
    attackZone.SetActive(true);
    attack.Enable();
    attack.performed += Attack;
    playerSpriteAnimator.runtimeAnimatorController = harpoonerAnimator;
    playerSpriteAnimator.Rebind();
    playerSpriteAnimator.Update(0f); // Force the animator to update immediately
  }
  private void OnDisable() {
    attackZone.SetActive(false);
    attack.performed -= Attack;
    attack.Disable();
    playerSpriteAnimator.runtimeAnimatorController = null;
    playerSpriteAnimator.Rebind();
    playerSpriteAnimator.Update(0f); // Force the animator to update immediately
  }
  private void Start() {
    enemyLayerMask = LayerMask.GetMask("Enemy");
  }
  #endregion
  #region Attack Logic
  /// <summary>
  /// Attacks in the direction of the mouse when the attack button is pressed.
  /// </summary>
  /// <param name="context"></param>
  public void Attack(InputAction.CallbackContext context) {

    Vector2 dir = MouseDirection();
    Direction direction;

    if (dir.y > Mathf.Abs(dir.x)) {
      // up: y is dominant and positive
      Debug.Log("Attack up");
      animator.SetTrigger("Attack-U");
      direction = Direction.up;
    } else if (dir.x > Mathf.Abs(dir.y)) {
      // right: x positive and dominant
      Debug.Log("Attack right");
      direction = Direction.right;
      animator.SetTrigger("Attack-R");
    } else if (-dir.x > Mathf.Abs(dir.y)) {
      // left: x negative and |x| dominant
      Debug.Log("Attack left");
      direction = Direction.left;
      animator.SetTrigger("Attack-L");
    } else {
      // dir.y is negative (downwards)—ignore or default
      return;
    }
    OnAttackAnimationEnd();
    onAttack?.Invoke(direction);
  }
  #endregion
  #region Animation
  public void OnAttackAnimationEnd() {
    animator.SetTrigger("Attack");
  }
  #endregion
  #region Helper Function
  /// <summary>
  /// Provides direction of mouse in world space.
  /// </summary>
  /// <returns>The direction of the mouse relative to the player model.</returns>
  private Vector2 MouseDirection() {
    // inside your Update or Attack()…
    Vector3 mouseScreen = Input.mousePosition;
    // 1) get direction
    Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    Vector2 dir = mouseWorld - transform.position;
    dir.Normalize();
    return dir;
  }
  #endregion

}
