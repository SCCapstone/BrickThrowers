// Copyright 2025 Brick Throwers
// PlayerAnimController.cs - Controls the player animations based on input.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAnimController : MonoBehaviour {
  // Animator
  public Animator playerAnimator;

  // Input actions
  public PlayerInputActions playerControls;
  private InputAction move;

  #region Setup Functions
  private void Awake() {
    playerControls = new PlayerInputActions();
  }
  private void OnEnable() {
    move = playerControls.Player.Move;
    move.Enable();
  }
  private void OnDisable() {
    move.Disable();
  }
  #endregion
  private void Update() {

    Vector2 input = move.ReadValue<Vector2>();

    if (input.y > 0) {
      playerAnimator.SetBool("MoveUp", true);
      playerAnimator.SetBool("MoveLeft", false);
      playerAnimator.SetBool("MoveRight", false);
    } else if (input.x < 0) {
      playerAnimator.SetBool("MoveUp", false);
      playerAnimator.SetBool("MoveLeft", true);
      playerAnimator.SetBool("MoveRight", false);
    } else if (input.x > 0) {
      playerAnimator.SetBool("MoveUp", false);
      playerAnimator.SetBool("MoveLeft", false);
      playerAnimator.SetBool("MoveRight", true);
    } else {
      playerAnimator.SetBool("MoveUp", false);
      playerAnimator.SetBool("MoveLeft", false);
      playerAnimator.SetBool("MoveRight", false);
    }
  }
}
