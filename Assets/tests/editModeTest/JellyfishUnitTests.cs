// File: Assets/Tests/editmodetest/JellyfishUnitTests.cs
// To run these EditMode tests:
//   In the Unity Editor: Window → General → Test Runner → select “EditMode” and click Run All
//   Via CLI:
//     Unity -batchmode -projectPath . -runTests -testPlatform EditMode -logFile -testResults TestResults/EditModeResults.xml

using NUnit.Framework;
using UnityEngine;
using System.Reflection;

[TestFixture]
public class JellyfishUnitTests
{
    private GameObject jellyObj;
    private Jellyfish jelly;

    [SetUp]
    public void SetUp()
    {
        jellyObj = new GameObject();
        jelly = jellyObj.AddComponent<Jellyfish>();
        jellyObj.AddComponent<Rigidbody2D>();
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(jellyObj);
    }

    [Test]
    public void DefaultValues_AreCorrect()
    {
        Assert.AreEqual(1.5f, jelly.floatSpeed, "floatSpeed must default to 1.5f");
        Assert.AreEqual(2f, jelly.directionChangeInterval, "directionChangeInterval must default to 2f");
        Assert.AreEqual(2f, jelly.stunDuration, "stunDuration must default to 2f");
    }

    [Test]
    public void ChangeDirection_SetsNormalizedMoveDirection()
    {
        // Call private ChangeDirection()
        MethodInfo changeDir = typeof(Jellyfish).GetMethod("ChangeDirection", BindingFlags.NonPublic | BindingFlags.Instance);
        changeDir.Invoke(jelly, null);

        // Retrieve private field moveDirection
        FieldInfo dirField = typeof(Jellyfish).GetField("moveDirection", BindingFlags.NonPublic | BindingFlags.Instance);
        Vector2 dir = (Vector2)dirField.GetValue(jelly);

        Assert.AreEqual(1f, dir.magnitude, 1e-3f, "moveDirection should be unit length after ChangeDirection");
    }
}
