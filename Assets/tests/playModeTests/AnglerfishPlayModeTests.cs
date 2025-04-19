// File: Assets/Tests/playmodetest/AnglerfishPlayModeTests.cs
// To run these PlayMode tests:
//   In the Unity Editor: open Window → General → Test Runner, select “PlayMode” and click Run All
//   Via CLI:
//     Unity -batchmode -projectPath . -runTests -testPlatform PlayMode \
//       -logFile -testResults TestResults/PlayModeResults.xml

using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class AnglerfishPlayModeTests
{
    private GameObject fishObj;
    private Anglerfish fish;
    private Rigidbody2D rb;
    private GameObject playerObj;
    private Player player;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        playerObj = new GameObject("Player");
        playerObj.tag = "Player";
        player = playerObj.AddComponent<Player>();
        player.oxygen = 5f;

        fishObj = new GameObject("Anglerfish");
        rb = fishObj.AddComponent<Rigidbody2D>();
        fish = fishObj.AddComponent<Anglerfish>();

        // assign light so Start does not error
        var light = fishObj.AddComponent<Light>();
        fish.anglerLight = light;

        yield return null; // allow Start
    }

    [UnityTearDown]
    public IEnumerator TearDown()
    {
        Object.Destroy(playerObj);
        Object.Destroy(fishObj);
        yield return null;
    }

    [UnityTest]
    public IEnumerator Swim_SetsVelocityMagnitude()
    {
        // let one frame run so Update sets velocity
        yield return null;
        float speed = rb.velocity.magnitude;
        Assert.AreEqual(fish.swimSpeed, speed, 0.1f);
    }

    [UnityTest]
    public IEnumerator DetectAndInteract_ReducesPlayerOxygen()
    {
        // place fish near player
        fishObj.transform.position = Vector3.zero;
        playerObj.transform.position = Vector3.right * (fish.detectionRange - 1f);

        // one frame to detect and interact
        yield return null;
        yield return null;

        Assert.Less(player.oxygen, 5f);
    }
}
