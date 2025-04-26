// File: Assets/Tests/playmodetest/SharkPlayModeTests.cs
// To run this specific PlayMode test only:
//   • In the Unity Editor Test Runner:
//       – Window → General → Test Runner  
//       – Select the “PlayMode” category  
//       – Right-click “SharkPlayModeTests” → Run Selected  
//   • Via CLI (runs only SharkPlayModeTests):
//       Unity -batchmode -projectPath . -runTests -testPlatform PlayMode \
//         -testFilter SharkPlayModeTests -logFile -testResults TestResults/SharkPlayModeTests.xml

using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class SharkPlayModeTests
{
    private GameObject sharkObj;
    private Shark shark;
    private Rigidbody2D sharkRb;
    private GameObject playerObj;
    private Player player;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        sharkObj = new GameObject("Shark");
        sharkRb = sharkObj.AddComponent<Rigidbody2D>();
        shark = sharkObj.AddComponent<Shark>();

        // Create player
        playerObj = new GameObject("Player");
        playerObj.tag = "Player";
        var col = playerObj.AddComponent<BoxCollider2D>();
        playerObj.AddComponent<Rigidbody2D>();
        player = playerObj.AddComponent<Player>();
        player.oxygen = 100f;

        // Position for collision
        sharkObj.transform.position = Vector3.zero;
        playerObj.transform.position = Vector3.zero;

        yield return null;                  // let Start() run
        yield return new WaitForFixedUpdate(); // resolve physics
    }

    [UnityTearDown]
    public IEnumerator TearDown()
    {
        Object.Destroy(sharkObj);
        Object.Destroy(playerObj);
        yield return null;
    }

    [UnityTest]
    public IEnumerator OnCollisionEnter2D_ReducesPlayerOxygen()
    {
        float before = player.oxygen;
        yield return new WaitForFixedUpdate();
        Assert.Less(player.oxygen, before, "Player oxygen should decrease on collision with Shark");
    }
}
