// File: Assets/Tests/playmodetest/JellyfishPlayModeTests.cs
// To run this specific PlayMode test only:
//   • In the Unity Editor Test Runner:
//       – Window → General → Test Runner  
//       – Select the “PlayMode” category  
//       – Right-click “JellyfishPlayModeTests” → Run Selected  
//   • Via CLI (runs only JellyfishPlayModeTests):
//       Unity -batchmode -projectPath . -runTests -testPlatform PlayMode \
//         -testFilter JellyfishPlayModeTests -logFile -testResults TestResults/JellyfishPlayModeTests.xml

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
        yield return null; // allow Start() to initialize
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
        // After one frame, Update has applied the velocity
        yield return null;
        Assert.AreEqual(jelly.floatSpeed, rb.velocity.magnitude, 0.1f);
    }

    [UnityTest]
    public IEnumerator Jellyfish_ChangesDirection_AfterInterval()
    {
        // Capture initial direction
        FieldInfo dirField = typeof(Jellyfish)
            .GetField("moveDirection", BindingFlags.NonPublic | BindingFlags.Instance);
        Vector2 initial = (Vector2)dirField.GetValue(jelly);

        // Wait longer than the interval
        yield return new WaitForSeconds(jelly.directionChangeInterval + 0.1f);

        Vector2 updated = (Vector2)dirField.GetValue(jelly);
        Assert.AreNotEqual(initial, updated, "moveDirection should change after directionChangeInterval");
    }
}
