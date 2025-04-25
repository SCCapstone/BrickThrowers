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

  private void Update() {
    /*
     * On Update, we get the input from the player and set the animator parameters.
     * There are three parameters to set: MoveUp, MoveLeft, and MoveRight.
     * For the up direction on input key W, MoveUp is set to true. Everything else is set to false.
     * For the left direction on input key A, MoveLeft is set to true. Everything else is set to false.
     * For the right direction on input key D, MoveRight is set to true. Everything else is set to false.
     * For no movement, all three parameters are set to false.
     */

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
  #endregion

}
