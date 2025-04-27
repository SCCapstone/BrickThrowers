using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class AnglerFishPTest
{
    private GameObject fishObj;
    private Anglerfish fish;
    private Rigidbody2D fishRb;
    private BoxCollider2D fishCol;

    private GameObject playerObj;
    private Diver diver;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        // create anglerfish
        fishObj = new GameObject("Anglerfish");
        fishRb = fishObj.AddComponent<Rigidbody2D>();
        fishCol = fishObj.AddComponent<BoxCollider2D>();
        fishCol.isTrigger = false;
        fish = fishObj.AddComponent<Anglerfish>();
        var light = fishObj.AddComponent<Light>();
        fish.anglerLight = light;

        // create player/diver
        playerObj = new GameObject("Player");
        playerObj.tag = "Player";
        playerObj.AddComponent<BoxCollider2D>();
        playerObj.AddComponent<Rigidbody2D>();
        diver = playerObj.AddComponent<Diver>();
        diver.oxygenLevel = 10f;

        // overlap for collision
        fishObj.transform.position = Vector3.zero;
        playerObj.transform.position = Vector3.zero;

        yield return null;  
        yield return new WaitForFixedUpdate();
    }

    [UnityTearDown]
    public IEnumerator TearDown()
    {
        Object.Destroy(fishObj);
        Object.Destroy(playerObj);
        yield return null;
    }

    [UnityTest]
    public IEnumerator OnCollisionStay2D_DamagesDiverOxygen()
    {
        float before = diver.oxygenLevel;
        yield return new WaitForFixedUpdate();  
        Assert.Less(diver.oxygenLevel, before);
    }
}
