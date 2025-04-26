// File: Assets/Tests/playmodetest/OctopusPlayModeTests.cs
// To run this specific PlayMode test only:
//   • In the Unity Editor Test Runner:
//       – Window → General → Test Runner  
//       – Select “PlayMode” category  
//       – Right-click “OctopusPlayModeTests” → Run Selected  
//   • Via CLI (runs only OctopusPlayModeTests):
//       Unity -batchmode -projectPath . -runTests -testPlatform PlayMode \
//         -testFilter OctopusPlayModeTests -logFile -testResults TestResults/OctopusPlayModeTests.xml

using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class OctopusPlayModeTests
{
    private GameObject octoObj;
    private Octopus octo;
    private Rigidbody2D rb;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        octoObj = new GameObject("Octopus");
        rb = octoObj.AddComponent<Rigidbody2D>();
        octo = octoObj.AddComponent<Octopus>();
        yield return null;               // allow Start() and first frame
        yield return new WaitForFixedUpdate();  // let Roam() coroutine apply velocity
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
        // After the initial Roam, velocity magnitude should equal moveSpeed
        float speed = rb.velocity.magnitude;
        Assert.AreEqual(octo.moveSpeed, speed, 0.1f, "Octopus should roam at moveSpeed after Start");
        yield break;
    }
}
