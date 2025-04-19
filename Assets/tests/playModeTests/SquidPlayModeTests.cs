// File: Assets/Tests/playmodetest/SquidPlayModeTests.cs
// To run these PlayMode tests:
//  • In the Unity Editor: Window → General → Test Runner → Select “PlayMode” → Run All
//  • CLI: Unity -batchmode -projectPath . -runTests -testPlatform PlayMode -logFile -testResults TestResults/PlayModeResults.xml

using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class SquidPlayModeTests
{
    GameObject squidObj;
    GameObject playerObj;
    Squid squid;
    Player player;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        playerObj = new GameObject("Player");
        playerObj.tag = "Player";
        player = playerObj.AddComponent<Player>();
        player.isBlinded = false;

        squidObj = new GameObject("Squid");
        squidObj.AddComponent<Rigidbody2D>();
        squid = squidObj.AddComponent<Squid>();
        yield return null; // allow Start() to run
    }

    [UnityTearDown]
    public IEnumerator TearDown()
    {
        Object.Destroy(playerObj);
        Object.Destroy(squidObj);
        yield return null;
    }

    [UnityTest]
    public IEnumerator Squid_AmbushThenRetreat_MovesAwayAfterInk()
    {
        // Position squid within ambushRange
        squidObj.transform.position = Vector3.zero;
        playerObj.transform.position = Vector3.right * (squid.ambushRange * 0.9f);

        // Wait one frame so DetectPlayerAndAmbush flags isAmbushing
        yield return null;

        // Simulate several frames so squid ambushes and then retreats
        float total = 0f;
        Vector2 initialPos = squidObj.transform.position;
        while (total < squid.ambushRange * 0.5f)
        {
            yield return new WaitForFixedUpdate();
            total += Time.fixedDeltaTime;
        }

        Vector2 afterAmbush = squidObj.transform.position;
        Assert.AreNotEqual(initialPos, afterAmbush, "Squid should have moved toward player during ambush");

        // Wait retreatDuration to complete retreat
        yield return new WaitForSeconds(squid.retreatDuration + 0.1f);
        Vector2 afterRetreat = squidObj.transform.position;
        Assert.AreNotEqual(afterAmbush, afterRetreat, "Squid should have moved away during retreat");
    }

    [UnityTest]
    public IEnumerator Squid_ReleaseInk_BlindsPlayer()
    {
        squidObj.transform.position = Vector3.zero;
        playerObj.transform.position = Vector3.zero;

        // Bypass ambush logic: directly call ReleaseInk
        squid.ReleaseInk();
        yield return null;

        Assert.IsTrue(player.isBlinded, "Player must be blinded after ink release");
        Assert.AreEqual(squid.inkBlindDuration, player.blindDuration, "Blind duration should match inkBlindDuration");
    }
}
