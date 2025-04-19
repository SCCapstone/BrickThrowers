// File: Assets/Tests/editmodetest/SquidUnitTests.cs
// To run these EditMode tests:
//  • In the Unity Editor: Window → General → Test Runner → Select “EditMode” → Run All
//  • CLI: Unity -batchmode -projectPath . -runTests -testPlatform EditMode -logFile -testResults TestResults/EditModeResults.xml

using NUnit.Framework;
using UnityEngine;

[TestFixture]
public class SquidUnitTests
{
    GameObject squidObj;
    Squid squid;

    [SetUp]
    public void SetUp()
    {
        squidObj = new GameObject();
        squid = squidObj.AddComponent<Squid>();
        squidObj.AddComponent<Rigidbody2D>();
        // Give it a fake player target
        var playerObj = new GameObject();
        playerObj.tag = "Player";
        playerObj.transform.position = Vector2.zero;
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(squidObj);
    }

    [Test]
    public void StartRetreat_SetsRetreatState()
    {
        Vector2 dir = Vector2.right;
        squid.StartRetreat(dir);
        Assert.IsFalse(squid.isAmbushing, "isAmbushing should be false after StartRetreat");
        Assert.IsTrue(squid.isRetreating, "isRetreating should be true after StartRetreat");
        Assert.AreEqual(-dir, squid.retreatDirection, "retreatDirection should be opposite of input");
        Assert.Greater(squid.retreatTimer, 0f, "retreatTimer should be set to retreatDuration");
    }

    [Test]
    public void ReleaseInk_BlindsPlayer()
    {
        var playerObj = GameObject.FindGameObjectWithTag("Player");
        var player = playerObj.AddComponent<Player>();
        float before = player.isBlinded ? 1f : 0f;
        squid.ReleaseInk();
        Assert.IsTrue(player.isBlinded, "Player should be blinded after ReleaseInk");
        Assert.AreEqual(squid.inkBlindDuration, player.blindDuration, "Blind duration must match inkBlindDuration");
    }

    [Test]
    public void DetectPlayerAndAmbush_SetsAmbushWhenInRange()
    {
        // place squid near player
        squidObj.transform.position = Vector3.zero;
        var playerObj = GameObject.FindGameObjectWithTag("Player");
        playerObj.transform.position = Vector3.right * squid.ambushRange * 0.9f;
        squid.DetectPlayerAndAmbush();
        Assert.IsTrue(squid.isAmbushing, "isAmbushing should be true when player is within ambushRange");
    }
}
