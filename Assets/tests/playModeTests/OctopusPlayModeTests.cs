// File: Assets/Tests/playmodetest/OctopusPlayModeTests.cs
// To run these PlayMode tests:
//   In the Unity Editor: Window → General → Test Runner → select “PlayMode” and click Run All
//   Via CLI:
//     Unity -batchmode -projectPath . -runTests -testPlatform PlayMode -logFile -testResults TestResults/PlayModeResults.xml

using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class OctopusPlayModeTests
{
    GameObject octoObj;
    Octopus octo;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        octoObj = new GameObject("Octopus");
        octoObj.AddComponent<Rigidbody2D>();
        octo = octoObj.AddComponent<Octopus>();
        yield return null; // allow Start to run
    }

    [UnityTearDown]
    public IEnumerator TearDown()
    {
        Object.Destroy(octoObj);
        yield return null;
    }

    [UnityTest]
    public IEnumerator Octopus_Roam_SetsVelocityMagnitude()
    {
        // After one frame, Update has called Roam
        yield return null;
        var rb = octoObj.GetComponent<Rigidbody2D>();
        float speed = rb.velocity.magnitude;
        Assert.AreEqual(octo.moveSpeed, speed, 0.1f);
    }
}
