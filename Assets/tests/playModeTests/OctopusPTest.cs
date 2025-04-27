// File: Assets/Tests/PlayMode/OctopusPTest.cs
// To run this specific PlayMode test only:
//   • In the Unity Editor Test Runner:
//       – Window → General → Test Runner  
//       – Select “PlayMode” category  
//       – Right-click “OctopusPTest” → Run Selected  
//   • Via CLI (runs only OctopusPTest):  
//       Unity -batchmode -projectPath . -runTests -testPlatform PlayMode \  
//         -testFilter OctopusPTest -logFile -testResults TestResults/OctopusPTest.xml

using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Reflection;

public class OctopusPTest
{
    private GameObject octoObj;
    private Octopus octo;
    private Rigidbody2D rb;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        // Create the Octopus and add a Rigidbody2D
        octoObj = new GameObject("Octopus");
        rb      = octoObj.AddComponent<Rigidbody2D>();
        octo    = octoObj.AddComponent<Octopus>();

        // Inject our Rigidbody2D into the private field so Roam() uses it
        typeof(Octopus)
            .GetField("rb", BindingFlags.NonPublic | BindingFlags.Instance)
            .SetValue(octo, rb);

        // Wait a frame for Start() and first Update()
        yield return null;
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
        // After one frame, the Roam coroutine should have set velocity
        yield return null;

        float speed = rb.velocity.magnitude;
        Assert.AreEqual(octo.moveSpeed, speed, 1f,
            "After one frame, octopus should be roaming at roughly moveSpeed");
    }
}
