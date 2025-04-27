// File: Assets/Tests/PlayMode/AnglerFishPTest.cs
// To run this specific PlayMode test only:
//   • In the Unity Editor Test Runner:
//       – Window → General → Test Runner  
//       – Select the “PlayMode” category  
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

    private GameObject playerObj;
    private Diver diver;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        // Create Anglerfish with Rigidbody2D and Collider
        fishObj = new GameObject("Anglerfish");
        fishRb  = fishObj.AddComponent<Rigidbody2D>();
        fishRb.gravityScale = 0f;                       // disable gravity for test
        fishObj.AddComponent<BoxCollider2D>();
        fish      = fishObj.AddComponent<Anglerfish>();
        fish.anglerLight = fishObj.AddComponent<Light>();

        // Create Diver (player) out of collision range
        playerObj = new GameObject("Player");
        playerObj.tag = "Player";
        playerObj.AddComponent<BoxCollider2D>();
        var playerRb = playerObj.AddComponent<Rigidbody2D>();
        playerRb.gravityScale = 0f;
        diver = playerObj.AddComponent<Diver>();
        diver.oxygenLevel = 10f;

        // Position apart so first test is purely movement
        fishObj.transform.position   = Vector3.zero;
        playerObj.transform.position = Vector3.up * (fish.detectionRange + 1f);

        // Wait a frame for Start() and first Update()
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
        // After one frame, Update() should have applied swim movement
        yield return null;
        Assert.AreEqual(fish.swimSpeed, fishRb.velocity.magnitude, 0.1f,
            "After one frame, Anglerfish should swim at swimSpeed");
    }

    [UnityTest]
    public IEnumerator OnCollisionStay2D_DamagesDiverOxygen()
    {
        // Overlap both at the same position to trigger collision
        fishObj.transform.position   = Vector3.zero;
        playerObj.transform.position = Vector3.zero;

        float before = diver.oxygenLevel;
        yield return new WaitForFixedUpdate();  // let OnCollisionStay2D run

        Assert.Less(diver.oxygenLevel, before,
            "OnCollisionStay2D should reduce the diver's oxygen level");
    }
}
