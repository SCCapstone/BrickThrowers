using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour {
  /*
   * A shop item shows the item picture, and the pricing.
   * There is a button where the pricing is to purchase the item.
   * When the button is clicked, the item is purchased.
   * Send a signal to the shop manager to update the currency.
   * So, create an action to signal the shop manager that the purchase was successful.
   * Ensure that the item purchased appears in the player inventory in game scenes.
   * Update the currency in the shop manager.
   */

  // Item related variables
  private ItemClass item;

  // Game objects
  [SerializeField] private TextMeshProUGUI itemPrice;
  [SerializeField] private Image image;

  // Actions
  public static event Action<int, ItemClass> onItemPurchased;

  /// <summary>
  /// Invokes a signal to the shop manager that the item was purchased.
  /// </summary>
  public void ItemPurchased() {
    onItemPurchased?.Invoke(item.ReturnCost(), item);
  }

  /// <summary>
  /// Sets the item class to the item class in the database.
  /// </summary>
  /// <param name="item">A valid ItemClass object.</param>
  public void SetItem(ItemClass item) {
    this.item = item;
  }

  /// <summary>
  /// Changes text to match the item cost.
  /// </summary>
  public void SetItemCost() {
    itemPrice.text = item.ReturnCost().ToString();
  }

  /// <summary>
  /// Changes the sprite to match the item icon.
  /// </summary>
  public void SetSprite() {
    this.image.sprite = item.itemIcon;
  }
}
