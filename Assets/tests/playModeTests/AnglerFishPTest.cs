// File: Assets/Tests/PlayMode/AnglerFishPTest.cs
// To run this specific PlayMode test only:
//   • In the Unity Editor Test Runner:
//       – Window → General → Test Runner  
//       – Select “PlayMode” category  
//       – Right-click “AnglerFishPTest” → Run Selected  
//   • Via CLI (runs only AnglerFishPTest):
//       Unity -batchmode -projectPath . -runTests -testPlatform PlayMode \  
//         -testFilter AnglerFishPTest -logFile -testResults TestResults/AnglerFishPTest.xml

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

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        // Create Anglerfish with Rigidbody2D and Collider
        fishObj = new GameObject("Anglerfish");
        fishRb  = fishObj.AddComponent<Rigidbody2D>();
        fishRb.gravityScale = 0f;
        fishCol = fishObj.AddComponent<BoxCollider2D>();
        fishCol.isTrigger = false;
        fish    = fishObj.AddComponent<Anglerfish>();
        fish.anglerLight = fishObj.AddComponent<Light>();

        // Create Player without Diver component
        playerObj = new GameObject("Player");
        playerObj.tag = "Player";
        playerObj.AddComponent<BoxCollider2D>();
        playerObj.AddComponent<Rigidbody2D>();

        // Position overlap
        fishObj.transform.position   = Vector3.zero;
        playerObj.transform.position = Vector3.zero;

        // Let Start() and first Update() run
        yield return null;
    }

    [UnityTearDown]
    public IEnumerator TearDown()
    {
        Object.Destroy(fishObj);
        Object.Destroy(playerObj);
        yield return null;
    }

    [UnityTest]
    public IEnumerator Swim_SetsVelocityMagnitudeToSwimSpeed()
    {
        // After one more frame, movement should be applied
        yield return null;
        Assert.AreEqual(fish.swimSpeed, fishRb.velocity.magnitude, 0.1f,
            "After one frame, Anglerfish should swim at swimSpeed");
    }

    [UnityTest]
    public IEnumerator OnCollisionStay2D_WithoutDiver_LogsError()
    {
        // Expect the error log for missing Diver component
        LogAssert.Expect(LogType.Error,
            "The target object tagged 'Player' does not have a Diver component!");

        // Wait for collision callback
        yield return new WaitForFixedUpdate();
    }
}