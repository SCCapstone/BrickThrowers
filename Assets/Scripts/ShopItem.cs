using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour {

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
