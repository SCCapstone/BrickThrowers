// File: Assets/Tests/playmodetest/JellyfishPlayModeTests.cs
// To run these PlayMode tests:
//   In the Unity Editor: Window → General → Test Runner → select “PlayMode” and click Run All
//   Via CLI:
//     Unity -batchmode -projectPath . -runTests -testPlatform PlayMode -logFile -testResults TestResults/PlayModeResults.xml

using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Reflection;

public class JellyfishPlayModeTests
{
    private GameObject jellyObj;
    private Jellyfish jelly;
    private Rigidbody2D rb;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        jellyObj = new GameObject("Jellyfish");
        rb = jellyObj.AddComponent<Rigidbody2D>();
        jelly = jellyObj.AddComponent<Jellyfish>();
        yield return null; // allow Start() to run and initial direction set
    }

    [UnityTearDown]
    public IEnumerator TearDown()
    {
        Object.Destroy(jellyObj);
        yield return null;
    }

    [UnityTest]
    public IEnumerator Jellyfish_Float_SetsVelocityMagnitude()
    {
        // After one frame, Update has applied velocity
        yield return null;

        float speed = rb.velocity.magnitude;
        Assert.AreEqual(jelly.floatSpeed, speed, 0.1f);
    }

    [UnityTest]
    public IEnumerator Jellyfish_ChangesDirection_AfterInterval()
    {
        // Capture initial direction
        FieldInfo dirField = typeof(Jellyfish).GetField("moveDirection", BindingFlags.NonPublic | BindingFlags.Instance);
        Vector2 initialDir = (Vector2)dirField.GetValue(jelly);

        // Wait for longer than directionChangeInterval
        yield return new WaitForSeconds(jelly.directionChangeInterval + 0.1f);

        Vector2 newDir = (Vector2)dirField.GetValue(jelly);
        Assert.AreNotEqual(initialDir, newDir, "moveDirection should change after directionChangeInterval");
    }
}
