// File: Assets/Tests/playmodetest/LionfishPlayModeTests.cs
// To run this specific PlayMode test only:
//   • In the Unity Editor Test Runner:
//       – Window → General → Test Runner  
//       – Select “PlayMode” category  
//       – Right-click “LionfishPlayModeTests” → Run Selected  
//   • Via CLI (runs only LionfishPlayModeTests):
//       Unity -batchmode -projectPath . -runTests -testPlatform PlayMode \
//         -testFilter LionfishPlayModeTests -logFile -testResults TestResults/LionfishPlayModeTests.xml

using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using UnityEngine.TestTools.Utils;

public class LionfishPlayModeTests
{
    private GameObject fishObj;
    private Lionfish fish;
    private Rigidbody2D fishRb;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        fishObj = new GameObject("Lionfish");
        fishRb = fishObj.AddComponent<Rigidbody2D>();
        fishObj.AddComponent<BoxCollider2D>();
        fish = fishObj.AddComponent<Lionfish>();

        yield return null; // allow Start & first Update
    }

    [UnityTearDown]
    public IEnumerator TearDown()
    {
        Object.Destroy(fishObj);
        yield return null;
    }

    [UnityTest]
    public IEnumerator Patrol_SetsVelocityMagnitudeToPatrolSpeed()
    {
        // After one frame, Update has applied Patrol()
        yield return null;
        Assert.AreEqual(fish.patrolSpeed, fishRb.velocity.magnitude, 0.1f);
    }

    [UnityTest]
    public IEnumerator OnCollisionEnter2D_CallsApplyPoisonAndLogsMessage()
    {
        // Create a diver (player) with tag "Player"
        var diverObj = new GameObject("Diver");
        diverObj.tag = "Player";
        diverObj.AddComponent<Rigidbody2D>();
        diverObj.AddComponent<BoxCollider2D>();
        var diver = diverObj.AddComponent<Diver>();

        // Position overlapping
        fishObj.transform.position = Vector3.zero;
        diverObj.transform.position = Vector3.zero;

        // Expect the log
        LogAssert.Expect(LogType.Log, "Lionfish attacked the diver!");

        // Wait for physics collision
        yield return new WaitForFixedUpdate();

        Object.Destroy(diverObj);
    }
}
