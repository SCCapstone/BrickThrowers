// File: Assets/Tests/playmodetest/LionfishPlayModeTests.cs
// To run these PlayMode tests:
//   In the Unity Editor: Window → General → Test Runner → select “PlayMode” and click Run All
//   Via CLI:
//     Unity -batchmode -projectPath . -runTests -testPlatform PlayMode \
//       -logFile -testResults TestResults/PlayModeResults.xml

using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class LionfishPlayModeTests
{
    private GameObject fishObj;
    private Lionfish fish;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        fishObj = new GameObject("Lionfish");
        fishObj.AddComponent<Rigidbody2D>();
        fish = fishObj.AddComponent<Lionfish>();
        yield return null; // allow Start to run
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
        // After one frame, Update calls Patrol()
        yield return null;

        Rigidbody2D rb = fishObj.GetComponent<Rigidbody2D>();
        float speed = rb.velocity.magnitude;
        Assert.AreEqual(fish.patrolSpeed, speed, 0.1f);
    }
}
