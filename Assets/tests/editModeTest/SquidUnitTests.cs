// File: Assets/Tests/editmodetest/SquidUnitTests.cs
// To run this specific EditMode test only:
//   • In the Unity Editor Test Runner:
//       – Window → General → Test Runner  
//       – Select “EditMode” category  
//       – Right-click “SquidUnitTests” → Run Selected  
//   • Via CLI (runs only SquidUnitTests):
//       Unity -batchmode -projectPath . -runTests -testPlatform EditMode \
//         -testFilter SquidUnitTests -logFile -testResults TestResults/SquidUnitTests.xml

using NUnit.Framework;
using UnityEngine;
using System.Reflection;

[TestFixture]
public class SquidUnitTests
{
    private GameObject squidObj;
    private Squid squid;
    private Player player;

    [SetUp]
    public void SetUp()
    {
        // Create the Squid
        squidObj = new GameObject("Squid");
        squid = squidObj.AddComponent<Squid>();
        squidObj.AddComponent<Rigidbody2D>();

        // Create a Player tagged “Player” for ReleaseInk()
        var playerObj = new GameObject("Player");
        playerObj.tag = "Player";
        player = playerObj.AddComponent<Player>();
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(squidObj);
        foreach (var go in Object.FindObjectsOfType<GameObject>())
            if (go.tag == "Player")
                Object.DestroyImmediate(go);
    }

    [Test]
    public void DefaultValues_AreCorrect()
    {
        Assert.AreEqual(7f, squid.ambushSpeed, "ambushSpeed must default to 7f");
        Assert.AreEqual(10f, squid.retreatSpeed, "retreatSpeed must default to 10f");
        Assert.AreEqual(8f, squid.ambushRange, "ambushRange must default to 8f");
        Assert.AreEqual(3f, squid.inkBlindDuration, "inkBlindDuration must default to 3f");
        Assert.AreEqual(1.5f, squid.retreatDuration, "retreatDuration must default to 1.5f");
    }

    [Test]
    public void DetectPlayerAndAmbush_SetsIsAmbushingWhenInRange()
    {
        // Position squid and player within ambushRange
        squidObj.transform.position = Vector3.zero;
        GameObject.FindGameObjectWithTag("Player").transform.position = Vector3.right * (squid.ambushRange * 0.9f);

        // Call Start() to set targetPlayer
        typeof(Squid).GetMethod("Start", BindingFlags.NonPublic | BindingFlags.Instance)
                     .Invoke(squid, null);

        // Invoke DetectPlayerAndAmbush()
        typeof(Squid).GetMethod("DetectPlayerAndAmbush", BindingFlags.NonPublic | BindingFlags.Instance)
                     .Invoke(squid, null);

        // Check private field isAmbushing
        bool isAmbushing = (bool)typeof(Squid)
            .GetField("isAmbushing", BindingFlags.NonPublic | BindingFlags.Instance)
            .GetValue(squid);
        Assert.IsTrue(isAmbushing, "DetectPlayerAndAmbush() should set isAmbushing when player within range");
    }

    [Test]
    public void ReleaseInk_BlindsPlayer()
    {
        // Call Start() to set targetPlayer
        typeof(Squid).GetMethod("Start", BindingFlags.NonPublic | BindingFlags.Instance)
                     .Invoke(squid, null);

        // Invoke ReleaseInk()
        typeof(Squid).GetMethod("ReleaseInk", BindingFlags.NonPublic | BindingFlags.Instance)
                     .Invoke(squid, null);

        Assert.IsTrue(player.isBlinded, "ReleaseInk() should call Player.Blind()");
        Assert.AreEqual(squid.inkBlindDuration, player.blindDuration, "Blind duration must match inkBlindDuration");
    }
}
