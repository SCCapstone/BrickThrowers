// File: Assets/Tests/playmodetest/SquidPlayModeTests.cs
// To run this specific PlayMode test only:
//   • In the Unity Editor Test Runner:
//       – Window → General → Test Runner  
//       – Select “PlayMode” category  
//       – Right-click “SquidPlayModeTests” → Run Selected  
//   • Via CLI (runs only SquidPlayModeTests):
//       Unity -batchmode -projectPath . -runTests -testPlatform PlayMode \
//         -testFilter SquidPlayModeTests -logFile -testResults TestResults/SquidPlayModeTests.xml

using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class SquidPlayModeTests
{
    private GameObject squidObj;
    private Squid squid;
    private Rigidbody2D rb;
    private GameObject playerObj;
    private Player player;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        // Create Player
        playerObj = new GameObject("Player");
        playerObj.tag = "Player";
        player = playerObj.AddComponent<Player>();
        player.isBlinded = false;
        player.blindDuration = 0f;

        // Create Squid
        squidObj = new GameObject("Squid");
        rb = squidObj.AddComponent<Rigidbody2D>();
        squid = squidObj.AddComponent<Squid>();

        yield return null; // allow Start()
    }

    [UnityTearDown]
    public IEnumerator TearDown()
    {
        Object.Destroy(squidObj);
        Object.Destroy(playerObj);
        yield return null;
    }

    [UnityTest]
    public IEnumerator Squid_AmbushThenRetreat_MovesTowardThenAway()
    {
        // Put both within ambushRange
        squidObj.transform.position = Vector3.zero;
        playerObj.transform.position = Vector3.right * (squid.ambushRange * 0.9f);

        // One frame to detect and start ambush
        yield return null;
        Vector2 velDuring = rb.velocity;
        Assert.Greater(velDuring.magnitude, 0f, "During ambush, squid should move toward player");

        // Wait retreatDuration + small buffer
        yield return new WaitForSeconds(squid.retreatDuration + 0.1f);
        Vector2 velAfter = rb.velocity;
        Assert.Greater(velAfter.magnitude, 0f, "During retreat, squid should move away from player");
    }

    [UnityTest]
    public IEnumerator Squid_ReleaseInk_BlindsPlayer()
    {
        // Directly call ReleaseInk via reflection
        typeof(Squid).GetMethod("Start", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                     .Invoke(squid, null);

        typeof(Squid).GetMethod("ReleaseInk", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                     .Invoke(squid, null);

        yield return null;
        Assert.IsTrue(player.isBlinded, "Player should be blinded after ink release");
        Assert.AreEqual(squid.inkBlindDuration, player.blindDuration);
    }
}
