using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{

    // Game objects
    [SerializeField] private GameObject shopOverlay;
    [SerializeField] private GameObject buyOptionPrefab;
    [SerializeField] private GameObject contentDisplay;

    // Database
    [SerializeField] private ItemDatabaseObject purchaseDatabase;

    // Currency manager
    [SerializeField] private CurrencyManager currency;

    // Actions
    public static event Action<ItemClass> onItemPurchased;
    
    void Awake()
    {
        shopOverlay.SetActive(false);
        for (int i = 0; i < purchaseDatabase.itemsDatabase.Length; i++)
        {
            PopulateStore(i);
        }
    }

    private void OnEnable()
    {
        ShopItem.onItemPurchased += UpdatePurchase;
    }
    

    private void PopulateStore(int i)
    {
        // Instantiate the prefab.
        GameObject buyOption = Instantiate(buyOptionPrefab, contentDisplay.transform);

        // Set the item class of the buy option prefab.
        buyOption.GetComponent<ShopItem>().SetItem(purchaseDatabase.itemsDatabase[i]);

        // Set the item cost of the buy option prefab.
        buyOption.GetComponent<ShopItem>().SetItemCost();

        // Set sprite
        buyOption.GetComponent<ShopItem>().SetSprite();
    }

    private void UpdatePurchase(int cost, ItemClass item)
    {
        /*
         * What must be done:
         * 1. Check if the player has enough currency. If yes, proceed. No, do nothing.
         * 2. Update the currency.
         * 3. Make a record of the purchase in a GameObject that will not be destroyed on load.
         * 
         * Q: Should I let then purchase the same item repeatedly? A: I really want to, because it would be funny. It is their money.
         */

        // Just in case, find the Game Object that has the currency manager and assign it.
        if (currency == null)
        {
            currency = FindObjectOfType<CurrencyManager>();
        }

        Currency playerCurrency = currency.ReturnCurrency();

        // Check if the player does not have enough currency. If so, do nothing.
        if (playerCurrency.CurrencyAmount < cost)
        {
            Debug.Log("Not enough currency.");
            return;
        }

        // Update the currency.
        currency.UpdateCurrency(-cost);

        // Make a record of the purchase in a GameObject that will not be destroyed on load.
        onItemPurchased?.Invoke(item);
    }

}
