// Copyright 2025 Brick Throwers
// // ItemSceneTransporter.cs - This script is used to transport items between scenes. Used by the shop.
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ItemSceneTransporter : MonoBehaviour {
  // List
  private static List<ItemClass> itemList = new List<ItemClass>();

  // Scene name
  private string lobbySceneName;
  private string activeSceneName;

  // Actions
  public static event Action<List<ItemClass>> startingItemsArrived;

  void Awake() {
    lobbySceneName = SceneManager.GetActiveScene().name;
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

    if (activeSceneName != lobbySceneName) {
      Debug.Log($"Scene changed to {activeSceneName}.");
      Debug.Log($"Items in the list: {itemList.Count}");
      // Check if the list is empty. If not, send the items to the scene.
      if (itemList.Count <= 0) {
        Debug.Log("Nothing in here.");
      } else {
        startingItemsArrived?.Invoke(itemList);
      }
    }
    itemList.Clear();
  }
}
