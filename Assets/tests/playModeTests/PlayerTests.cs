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
    

 
}