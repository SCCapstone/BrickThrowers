using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollectManager : MonoBehaviour
{
    // Inventory manager for player
    [SerializeField]
    private InventoryManager inventory;

    // Item Interaction
    public KeyCode pickUpItemKey = KeyCode.E;
    public KeyCode dropItemKey = KeyCode.Q;

    // Inventory
    private bool nearItem; // If the player is near an item, this is true.
    public List<GameObject> nearestItems = new List<GameObject>();


    // Start is called before the first frame update
    /// <summary>
    /// If the player is near an item, switch to true.
    /// </summary>
    /// <param name="collision"></param>
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Item"))
        {
            nearestItems.Add(collision.gameObject);
            nearItem = true;
        }
    }

    /// <summary>
    /// If the player has left item vicinity, this is false.
    /// </summary>
    /// <param name="collision"></param>
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Item"))
        {
            // find the gameObject within the nearest items list, and then remove it.
            nearestItems.Remove(collision.gameObject);
            nearItem = false;
        }
    }

    /*
     * When the player clicks the pick up key, the item is added into the inventory.
     */
    public void Update()
    {
        if (Input.GetKeyDown(pickUpItemKey) && nearItem && GameObject.FindWithTag("Item"))
        {
            // Add the item to the inventory
            bool success = inventory.Add(nearestItems[0].GetComponent<GameItem>().gameItem);
            if (success)
            {
                Destroy(nearestItems[0]);
            }
            inventory.RefreshUI();
        }
        if (Input.GetKeyDown(dropItemKey))
        {
            inventory.DropItem();
        }

    }
}
