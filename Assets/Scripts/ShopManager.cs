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
    private CurrencyManager currency;

    // Start is called before the first frame update
    void Start()
    {
        shopOverlay.SetActive(false);
        for (int i = 0; i < purchaseDatabase.itemsDatabase.Length; i++)
        {
            PopulateStore(i);
        }
    }

    /*
     * Before the game loads, populate the store with the available purchasable items.
     * 1. Create a prefab for the item.
     * 2. Set that buy option prefab's item class to the item class in the database.
     * 3. Add the buy option prefab to the content display.
     * 4. Continue doing the above until all items are added to the store. Effectively going through the database.
     */

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


}
