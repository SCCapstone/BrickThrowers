using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopManager : MonoBehaviour {
  // Game objects
  [SerializeField]
  private GameObject shopOverlay;

  [SerializeField]
  private GameObject buyOptionPrefab;

  [SerializeField]
  private GameObject contentDisplay;

  [SerializeField]
  private TextMeshProUGUI announceText;

  // Database
  [SerializeField]
  private ItemDatabaseObject purchaseDatabase;

  // Currency manager
  [SerializeField]
  private CurrencyManager currency;

  // Actions
  public static event Action<ItemClass> onItemAcquired;

  #region Setup
  void Awake() {
    shopOverlay.SetActive(false);
    for (int i = 0; i < purchaseDatabase.itemsDatabase.Length; i++) {
      PopulateStore(i);
    }
  }

  private void OnEnable() {
    ShopItem.onItemPurchased += UpdatePurchase;
  }

  private void OnDisable() {
    ShopItem.onItemPurchased -= UpdatePurchase;
  }

  private void Start() {
    if (announceText == null) {
      announceText = GameObject.Find("AnnouncementText").GetComponent<TextMeshProUGUI>();
    }
    announceText.text = "";
  }
  #endregion
  #region Purchase Logic
  /// <summary>
  /// Places an item from the Item Database resource into the shop.
  /// </summary>
  /// <param name="i">Index of the item database.</param>
  private void PopulateStore(int i) {
    // Instantiate the prefab.
    GameObject buyOption = Instantiate(buyOptionPrefab, contentDisplay.transform);

    // Set the item class of the buy option prefab.
    buyOption.GetComponent<ShopItem>().SetItem(purchaseDatabase.itemsDatabase[i]);

    // Set the item cost of the buy option prefab.
    buyOption.GetComponent<ShopItem>().SetItemCost();

    // Set sprite
    buyOption.GetComponent<ShopItem>().SetSprite();
  }

  /// <summary>
  /// Updates the currency and invokes the item purchased event.
  /// </summary>
  /// <param name="cost"></param>
  /// <param name="item"></param>
  private void UpdatePurchase(int cost, ItemClass item) {
    if (currency == null) {
      currency = FindObjectOfType<CurrencyManager>();
    }
    Currency playerCurrency = currency.ReturnCurrency();
    if (playerCurrency.CurrencyAmount < cost) {
      Debug.Log("Not enough currency.");
      StartCoroutine(Announce(item.itemName, false));
      return;
    }
    currency.UpdateCurrency(-cost);
    StartCoroutine(Announce(item.itemName, true));
    onItemAcquired?.Invoke(item);
  }

  private IEnumerator Announce(string itemName, bool purchaseSuccess) {
    if (purchaseSuccess) {
      announceText.text = $"You purchased {itemName}!";
    } else {
      announceText.text = $"You do not have enough currency to purchase {itemName}!";
    }
    yield return new WaitForSeconds(2);
    announceText.text = "";
  }
  #endregion
}
