using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
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
    public static event Action<int> onItemPurchased;

    // The item that is being purchased.
    private void ItemPurchased()
    {
        onItemPurchased?.Invoke(item.ReturnCost());
    }

    public void SetItem(ItemClass item)
    {
        this.item = item;
    }

    // Change the text to match item cost.
    public void SetItemCost()
    {
        itemPrice.text = item.ReturnCost().ToString();
    }

    public void SetSprite()
    {
        this.image.sprite = item.itemIcon;
    }
}
