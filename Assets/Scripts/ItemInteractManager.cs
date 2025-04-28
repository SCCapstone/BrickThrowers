/*
 * Copyright 2025 Scott Do
 * 2/15/2025
 * Controls the item interaction for the player.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ItemInteractManager : MonoBehaviour {
  // Input actions
  private PlayerInputActions inputActions;
  private InputAction pickUpItemAction;
  private InputAction dropItemAction;
  private InputAction useItemAction;

  // Inventory manager for player
  [SerializeField]
  private InventoryManager inventory;

  // Item Interaction
  public KeyCode pickUpItemKey = KeyCode.E;
  public KeyCode dropItemKey = KeyCode.Q;
  public KeyCode useItemKey = KeyCode.R;

  // Inventory
  public List<GameObject> nearestItems = new List<GameObject>();

  #region Action Initialization

  private void Awake() {
    inputActions = new PlayerInputActions();
  }

  private void OnEnable() {
    pickUpItemAction = inputActions.Player.PickUp;
    dropItemAction = inputActions.Player.Drop;
    useItemAction = inputActions.Player.Use;
    pickUpItemAction.Enable();
    dropItemAction.Enable();
    useItemAction.Enable();
  }

  private void OnDisable() {
    pickUpItemAction.Disable();
    dropItemAction.Disable();
    useItemAction.Disable();
  }

  #endregion

  #region Collision

  // Start is called before the first frame update
  /// <summary>
  /// If the player is near an item, switch to true.
  /// </summary>
  /// <param name="collision"></param>
  public void OnTriggerEnter2D(Collider2D collision) {
    if (collision.gameObject.CompareTag("Item")) {
      nearestItems.Add(collision.gameObject);
    }
  }

  /// <summary>
  /// If the player has left item vicinity, this is false.
  /// </summary>
  /// <param name="collision"></param>
  public void OnTriggerExit2D(Collider2D collision) {
    if (collision.gameObject.CompareTag("Item")) {
      // find the gameObject within the nearest items list, and then remove it.
      nearestItems.Remove(collision.gameObject);
    }
  }
  #endregion

  /*
   * When the player clicks the pick up key, the item is added into the inventory.
   */
  public void Update() {
    if (pickUpItemAction.WasPressedThisFrame() && GameObject.FindWithTag("Item")) {
      // Add the item to the inventory
      bool success = inventory.Add(nearestItems[0].GetComponent<GameItem>().gameItemClass);

      if (success) {
        Destroy(nearestItems[0]);
      }
    }
    if (dropItemAction.WasPressedThisFrame()) {
      inventory.DropItem();
    }
    if (useItemAction.WasPerformedThisFrame()) {
      inventory.UseItem();
    }
  }
}
