using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UIElements;


public class PlayerTests
{

    
    private GameObject antidoteItem = Resources.Load<GameObject>("antidoteTest");
    private GameObject playerPrefab = Resources.Load<GameObject>("TempPlayerTest");

    private GameObject playerObject;
    private Player player;

    private GameObject itemObject;
    

    
    
    [SetUp]
    public void Setup()
    {
        CreatePlayer();
        if(Camera.main == null)
        {
            GameObject cam = new GameObject("Main Camera");
            Camera cameraComponent = cam.AddComponent<Camera>();
            cam.tag = "MainCamera";
            cam.transform.position = new Vector3(0,0,-50);
        }
      
    }
    
    
   

    
    private void CreatePlayer()
    {
        playerObject = GameObject.Instantiate(playerPrefab);
        player = playerObject.GetComponent<Player>();

    }
    

    /*
    private void CreateItem()
    {
        itemObject = GameObject.Instantiate(antidoteItem);
        item = itemObject.GetComponent<GroundItem>();

    }
    */

    //Asserts player is properly loaded
    //Asserts player is affected by water
     [UnityTest]
    public IEnumerator PlayTest(){
        
        Transform playerTransform = playerObject.transform;
        yield return new WaitForSeconds(1f);
        Assert.NotNull(playerObject);
        float startPos =playerTransform.position.y;
        yield return new WaitForSeconds(1f);
        float newPos = playerTransform.position.y;
        
        Assert.AreNotEqual(startPos,newPos);
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
