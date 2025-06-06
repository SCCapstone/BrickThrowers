/*
 * Copyright 2025 Brick Throwers
 * 2/15/2025
 * This script is used to manage the player's inventory.
 */
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour {
  // Constants
  private const int SLOT_DISTANCE = 110;

  // Input actions
  private PlayerInputActions inputActions;
  private InputAction moveInventory;

  // Item Management
  [SerializeField]
  private SlotClass[] startingItems;
  private SlotClass[] items;
  private GameObject[] slots;
  private int currentPos = 0;

  // Prefabs and Game Objects
  [SerializeField]
  private GameObject slotHolder;

  [SerializeField]
  private Player player;

  // Cursor delay
  public float moveDelay = 0.2f;
  private Coroutine moveCoroutine;

  [SerializeField] private LevelManager instance;

  #region Setup

  private void Start() {
    slots = new GameObject[slotHolder.transform.childCount];
    items = new SlotClass[slots.Length];

    for (int i = 0; i < slots.Length; i++) {
      items[i] = new SlotClass();
    }

    for (int i = 0; i < startingItems.Length; i++) {
      items[i] = startingItems[i];
    }

    for (int i = 0; i < slotHolder.transform.childCount; i++) {
      slots[i] = slotHolder.transform.GetChild(i).gameObject;
    }
  }

  private void Awake() {
    inputActions = new PlayerInputActions();
  }

  private void OnEnable() {
    moveInventory = inputActions.Player.InventoryManagement;
    moveInventory.Enable();

    moveInventory.performed += OnMovePerformed;
    moveInventory.canceled += OnMoveCanceled;
    ItemSceneTransporter.startingItemsArrived += AddStartingItems;

  }

  private void OnDisable() {
    moveInventory.performed -= OnMovePerformed;
    moveInventory.canceled -= OnMoveCanceled;
    ItemSceneTransporter.startingItemsArrived -= AddStartingItems;

    moveInventory.Disable();
  }

  private void Update() {
    RefreshUI();
  }

  #endregion

  #region Inventory Utilities
  /// <summary>
  /// Refreshes the inventory UI.
  /// </summary>
  [ContextMenu("Refresh Inventory")]
  public void RefreshUI() {
    for (int i = 0; i < slots.Length; i++) {
      try {
        slots[i].transform.GetChild(0).GetComponent<Image>().sprite = items[i]
            .Item
            .itemIcon;
        slots[i].transform.GetChild(0).GetComponent<Image>().enabled = true;
      } catch {
        slots[i].transform.GetChild(0).GetComponent<Image>().sprite = null;
        slots[i].transform.GetChild(0).GetComponent<Image>().enabled = false;
      }
    }
  }

  /// <summary>
  /// Adds an item into a player's inventory.
  /// </summary>
  /// <param name="item"></param>
  /// <returns></returns>
  public bool Add(ItemClass item) {
    // First, check if the item is valid. If not, return false.
    if (item == null)
      return false;
    // Then, determine if the inventory is full. If it is, return false.
    if (items[items.Length - 1].Item != null) {
      Debug.Log("too many items mate.");
      return false;
    }

    // If the inventory is not full, add the item to the first available slot.
    for (int i = 0; i < items.Length; i++) {
      if (items[i].Item == null) {
        items[i] = new SlotClass(item);
        break;
      }
    }

    return true;
  }

  /// <summary>
  /// Removes an item from a player's inventory.
  /// </summary>
  /// <param name="item"></param>
  /// <returns></returns>
  private ItemClass Remove(ItemClass item) {
    int slotToRemove = 0;

    for (int i = 0; i < items.Length; i++) {
      if (items[i].Item == item) {
        slotToRemove = i;
        break;
      }
    }
    ItemClass temp = items[slotToRemove].Item;
    items[slotToRemove].Clear();

    RefreshUI();
    return temp;
  }
  /// <summary>
  /// Drops the item at the cursor position in the inventory.
  /// </summary>
  /// <returns></returns>
  public bool DropItem() {
    // First, at the cursor, determine if there is an item. If not, disregard the command.
    if (items[currentPos].Item == null)
      return false;

    // Else there is an item, remove it from the inventory and drop it in the world.
    // Start by keeping a reference to the item to gain access to the GameObject prefab.
    // Then, instantiate the item in the world.
    ItemClass temp = Remove(items[currentPos].Item);
    // Instantiate the item in the world relative to the player's position; it should be in front of the player.
    Instantiate(
        temp.prefab,
        new Vector3(
            transform.parent.position.x + 1,
            transform.parent.position.y,
            transform.parent.position.z
        ),
        Quaternion.identity
    );
    return true;
  }

  /// <summary>
  /// Determines if the inventory contains a specific item.
  /// </summary>
  /// <param name="item"></param>
  /// <returns>True if the item is in the inventory, otherwise false.</returns>
  public bool Contains(ItemClass item) {
    foreach (SlotClass slot in items) {
      if (slot != null && item == (slot.Item)) {
        return true;
      }
    }
    return false;
  }

  #endregion // Inventory Utilities

  #region Arrow Cursor Navigation
  /// <summary>
  /// Moves the cursor based on the input value.
  /// </summary>
  /// <param name="context"></param>
  private void OnMovePerformed(InputAction.CallbackContext context) {
    float moveValue = context.ReadValue<float>();

    if (moveCoroutine != null)
      StopCoroutine(moveCoroutine);

    moveCoroutine = StartCoroutine(MoveCursorRepeatedly(moveValue));
  }

  /// <summary>
  /// Stops the coroutine when the key is released.
  /// </summary>
  /// <param name="context"></param>
  private void OnMoveCanceled(InputAction.CallbackContext context) {
    if (moveCoroutine != null) {
      StopCoroutine(moveCoroutine);
      moveCoroutine = null;
    }
  }

  /// <summary>
  /// Runs the cursor movement repeatedly while the key is held down.
  /// </summary>
  /// <param name="moveValue"></param>
  /// <returns></returns>
  private IEnumerator MoveCursorRepeatedly(float moveValue) {
    MoveCursor(moveValue);

    while (true) {
      yield return new WaitForSeconds(moveDelay);
      MoveCursor(moveValue);
    }
  }

  /// <summary>
  /// Determines which direction to move the cursor.
  /// </summary>
  /// <param name="moveValue"></param>
  private void MoveCursor(float moveValue) {
    if (moveValue > 0) {
      MoveCursorRight();
    } else if (moveValue < 0) {
      MoveCursorLeft();
    }
  }

  /// <summary>
  /// Moves the cursor to the right.
  /// </summary>
  /// <returns></returns>
  private bool MoveCursorRight() {
    try {
      slots[currentPos].transform.GetChild(1).GetComponent<Image>().enabled = false;
      currentPos++;

      if (currentPos > slots.Length - 1) {
        currentPos = 0;
      }
      slots[currentPos].transform.GetChild(1).GetComponent<Image>().enabled = true;
      return true;
    } catch (Exception e) {
      Debug.Log(e.Message);
      return false;
    }
  }

  /// <summary>
  /// Moves the cursor to the left.
  /// </summary>
  /// <returns></returns>
  private bool MoveCursorLeft() {
    try {
      slots[currentPos].transform.GetChild(1).GetComponent<Image>().enabled = false;
      currentPos--;

      if (currentPos < 0) {
        currentPos = slots.Length - 1;
      }
      slots[currentPos].transform.GetChild(1).GetComponent<Image>().enabled = true;
      return true;
    } catch (Exception e) {
      Debug.Log(e.Message);
      return false;
    }
  }
  #endregion

  #region Item Using
  /// <summary>
  /// Use the items in the current cursor.
  /// </summary>
  /// <returns>The success or failure of the operation.</returns>
  public bool UseItem() {
    if (items[currentPos].Item == null) {
      Debug.Log("you have no item");
      return false;
    }

    bool itemUsed = items[currentPos].Item.Use(player);
    if (items[currentPos].Item.GetType() == typeof(ConsumableClass) && itemUsed) {
      // Destroy the item after it has been used.
      Remove(items[currentPos].Item);
    }
    return true;
  }
  #endregion
  /// <summary>
  ///  Adds the starting items to the inventory.
  /// </summary>
  /// <param name="startItems"></param>
  private void AddStartingItems(List<ItemClass> startItems) {
    // populate the starting array.
    // But first, initialize the starting array. The length is equivalent to the number of items in the List.
    // Once the array is initialized, add the items to the array.
    startingItems = new SlotClass[startItems.Count];
    for (int i = 0; i < startItems.Count; i++) {
      startingItems[i] = new SlotClass(startItems[i]);
    }
  }
  /// <summary>
  /// Removes all artifacts from the inventory.
  /// </summary>
  public void RemoveArtifacts() {
    foreach (SlotClass slot in items) {
      if (slot != null && slot.Item.GetType() == typeof(ArtifactClass)) {
        ArtifactClass artifact = slot.Item as ArtifactClass;
        if (artifact != null) {
          //LevelManager.Instance.AddScore(artifact.value);
          instance.AddScore(artifact.value);
          Remove(slot.Item);
          Debug.Log("Removing Artifact");
        } else {
          Debug.LogWarning("slot.Item is not an ArtifactClass.");
        }
      }
    }
  }
}
