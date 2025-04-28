// File: Assets/Tests/Editor/OctopusETest.cs
// To run this specific EditMode test only:
//   • In the Unity Editor Test Runner:
//       – Window → General → Test Runner  
//       – Select “EditMode” category  
//       – Right-click “OctopusETest” → Run Selected  
//   • Via CLI (runs only OctopusETest):  
//       Unity -batchmode -projectPath . -runTests -testPlatform EditMode \  
//         -testFilter OctopusETest -logFile -testResults TestResults/OctopusETest.xml

using System.Collections;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;

[TestFixture]
public class OctopusETest 
{
    private GameObject octoObj;
    private Octopus octo;
    private Rigidbody2D rb;

    [SetUp]
    public void SetUp()
    {
        // Create Octopus GameObject and attach Rigidbody2D
        octoObj = new GameObject("Octopus");
        rb      = octoObj.AddComponent<Rigidbody2D>();
        octo    = octoObj.AddComponent<Octopus>();

        // Inject the private rb field so Roam() can use it
        typeof(Octopus)
            .GetField("rb", BindingFlags.NonPublic | BindingFlags.Instance)
            .SetValue(octo, rb);
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(octoObj);
    }

    [Test]
    public void DefaultValues_AreCorrect()
    {
        Assert.AreEqual(3f, octo.detectionRange,  "detectionRange must default to 3f");
        Assert.AreEqual(5f, octo.latchDuration,    "latchDuration must default to 5f");
        Assert.AreEqual(60,  octo.health,          "health must default to 60");
        Assert.AreEqual(15f, octo.moveSpeed,       "moveSpeed must default to 15f");

        // private roamDuration default is 2f
        var roamField = typeof(Octopus)
            .GetField("roamDuration", BindingFlags.NonPublic | BindingFlags.Instance);
        float roamDur = (float)roamField.GetValue(octo);
        Assert.AreEqual(2f, roamDur, "roamDuration must default to 2f");
    }

    [Test]
    public void TakeDamage_ReducesHealth()
    {
        int before = octo.health;
        octo.TakeDamage(10);
        Assert.AreEqual(before - 10, octo.health, "TakeDamage should subtract from health");
    }

    [Test]
    public void RoamCoroutine_SetsIsRoamingTrueAndVelocity()
    {
        // Call the private Roam() coroutine
        var roamMethod = typeof(Octopus)
            .GetMethod("Roam", BindingFlags.NonPublic | BindingFlags.Instance);
        var enumerator = (IEnumerator)roamMethod.Invoke(octo, null);

        // First yield: after setting isRoaming and velocity
        Assert.IsTrue(enumerator.MoveNext(), "Roam coroutine should yield at least once");

        // Check isRoaming flag
        var flagField = typeof(Octopus)
            .GetField("isRoaming", BindingFlags.NonPublic | BindingFlags.Instance);
        bool isRoaming = (bool)flagField.GetValue(octo);
        Assert.IsTrue(isRoaming, "isRoaming should be true at start of Roam()");

        // Check velocity magnitude
        float speed = rb.velocity.magnitude;
        Assert.AreEqual(octo.moveSpeed, speed, 1e-3f, "Roam() should set rb.velocity to moveSpeed");
    }
}
