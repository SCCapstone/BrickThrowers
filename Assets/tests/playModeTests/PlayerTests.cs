using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PlayerTests
{
    private GameObject antidoteItem = Resources.Load<GameObject>("antidoteTest");
    private GameObject playerPrefab = Resources.Load<GameObject>("TempPlayerTest");
    private ItemDatabaseObject itemDatabase = Resources.Load<ItemDatabaseObject>("Item Database");

    private GameObject playerObject;
    private Player player;

    private GameObject itemObject;
    private GroundItem item;

    [SetUp]
    public void Setup()
    {
        CreatePlayer();
        CreateItem();
    }

    private void CreatePlayer()
    {
        playerObject = GameObject.Instantiate(playerPrefab);
        player = playerObject.GetComponent<Player>();

    }

    private void CreateItem()
    {
        itemObject = GameObject.Instantiate(antidoteItem);
        item = itemObject.GetComponent<GroundItem>();

    }

    /*
     * Test to determine if a player can pick up an item.
     */
    [UnityTest]
    public IEnumerator PlayerCollectsEquipmentItem()
    {
        // Wait for user input to click key E.
        Debug.Log("Waiting for player to click key E to collect item.");
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));

        // Determine if the item is added into the inventory.
        Assert.IsFalse(itemObject.activeSelf);
        yield return null;
        Assert.IsTrue(player.inventory.Container.Items.Count > 0);
        Assert.AreEqual(itemDatabase.items[0].Id, player.inventory.Container.Items[0].item.Id);
        Assert.AreEqual(itemDatabase.items[0].itemName, player.inventory.Container.Items[0].item.Name);




    }
}
