// File: Assets/Tests/playmodetest/SharkPlayModeTests.cs
// To run these PlayMode tests:
//  • In the Unity Editor: Window → General → Test Runner → Select “PlayMode” → Run All
//  • CLI: Unity -batchmode -projectPath . -runTests -testPlatform PlayMode -logFile -testResults TestResults/PlayModeResults.xml

using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class SharkPlayModeTests
{
    GameObject sharkObj;
    GameObject playerObj;
    Player player;
    Shark shark;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        // Create a player with a Player component
        playerObj = new GameObject("Player");
        playerObj.tag = "Player";
        player = playerObj.AddComponent<Player>();
        player.oxygen = 100;

        // Create the shark
        sharkObj = new GameObject("Shark");
        sharkObj.AddComponent<Rigidbody2D>();
        shark = sharkObj.AddComponent<Shark>();
        yield return null; // wait one frame for Start()
    }

    [UnityTearDown]
    public IEnumerator TearDown()
    {
        Object.Destroy(playerObj);
        Object.Destroy(sharkObj);
        yield return null;
    }

    [UnityTest]
    public IEnumerator Shark_CollidingWithPlayer_ReducesOxygen()
    {
        // Position shark directly on player
        sharkObj.transform.position = playerObj.transform.position;
        var collider = sharkObj.AddComponent<CircleCollider2D>();
        collider.isTrigger = false;
        playerObj.AddComponent<CircleCollider2D>();

        // simulate physics for one frame
        yield return new WaitForFixedUpdate();
        
        // OnCollisionEnter2D should have fired
        Assert.Less(player.oxygen, 100, "Player oxygen should be reduced on collision");
        yield break;
    }

    [UnityTest]
    public IEnumerator Shark_ChargeTowardPlayer_IncreasesVelocityMagnitude()
    {
        // Place player within detection range
        sharkObj.transform.position = Vector3.zero;
        playerObj.transform.position = Vector3.right * (shark.detectionRange - 1f);

        // Wait one frame for Update to detect and start charging
        yield return null;
        yield return new WaitForFixedUpdate();

        var rb = sharkObj.GetComponent<Rigidbody2D>();
        float speed = rb.velocity.magnitude;
        Assert.Greater(speed, shark.patrolSpeed, "When charging, shark speed must exceed patrolSpeed");
        yield break;
    }
}
