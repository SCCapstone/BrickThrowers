// File: Assets/Tests/PlayMode/AnglerFishPTest.cs

using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class AnglerFishPTest
{
    private GameObject fishObj;
    private Anglerfish fish;
    private Rigidbody2D fishRb;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        fishObj = new GameObject("Anglerfish");
        fishRb  = fishObj.AddComponent<Rigidbody2D>();
        fishObj.AddComponent<BoxCollider2D>();
        fish = fishObj.AddComponent<Anglerfish>();
        fish.anglerLight = fishObj.AddComponent<Light>();

        // place player out of detection range
        var playerObj = new GameObject("Player");
        playerObj.tag = "Player";
        playerObj.AddComponent<BoxCollider2D>();
        playerObj.AddComponent<Rigidbody2D>();
        playerObj.transform.position = Vector3.up * (fish.detectionRange + 1f);

        yield return null;
    }

    [UnityTearDown]
    public IEnumerator TearDown()
    {
        Object.Destroy(fishObj);
        yield return null;
    }

    [UnityTest]
    public IEnumerator Swim_SetsVelocityMagnitudeToSwimSpeed()
    {
        // After one frame, Update should have applied the swim movement
        yield return null;
        Assert.AreEqual(fish.swimSpeed, fishRb.velocity.magnitude, 0.1f);
    }
}
