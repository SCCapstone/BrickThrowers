// Copyright 2025 Brick Throwers
// // ItemSceneTransporter.cs - This script is used to transport items between scenes. Used by the shop.
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ItemSceneTransporter : MonoBehaviour {
  // List
  [SerializeField] private List<ItemClass> itemList = new List<ItemClass>();

  // Singleton
  public static ItemSceneTransporter Instance { get; private set; }

  // Scene name
  private string lobbySceneName = "Lobby Scene";
  private string activeSceneName;

  // Actions
  public static event Action<List<ItemClass>> startingItemsArrived;

  void Awake() {
    if (Instance != null && Instance != this) {
      Destroy(gameObject);
      return;
    }
    Instance = this;
    DontDestroyOnLoad(this.gameObject);
  }

  private void OnEnable() {
    // Subscribe to the scene loaded event
    // Also need the onItemPurchased event from the ShopManager
    ShopManager.onItemAcquired += AddPurchasedItemToList;
    SceneManager.sceneLoaded += OnSceneChanged;
  }

  private void OnDisable() {
    ShopManager.onItemAcquired -= AddPurchasedItemToList;
    SceneManager.sceneLoaded -= OnSceneChanged;
  }

  /// <summary>
  /// Adds the purchased item to the list.
  /// </summary>
  /// <param name="item"></param>
  private void AddPurchasedItemToList(ItemClass item) {
    Debug.Log($"add item: {item.itemName}");
    itemList.Add(item);
  }
  /// <summary>
  /// Invokes signal to update the item list when a scene is loaded. InventoryManager.cs will
  /// listen for the signal.
  /// </summary>
  /// <param name="scene"></param>
  /// <param name="mode"></param>
  private void OnSceneChanged(Scene scene, LoadSceneMode mode) {
    activeSceneName = SceneManager.GetActiveScene().name;
    // If you are not in the lobby scene, call the signal.
    // Then, clear the list to avoid duplicates.
    if (activeSceneName != lobbySceneName) {
      startingItemsArrived?.Invoke(itemList);
      itemList.Clear();
    }
  }
}
