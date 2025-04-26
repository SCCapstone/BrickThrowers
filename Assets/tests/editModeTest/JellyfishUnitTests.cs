// File: Assets/Tests/editmodetest/JellyfishUnitTests.cs
// To run this specific EditMode test only:
//   • In the Unity Editor Test Runner:
//       – Window → General → Test Runner  
//       – Select the “EditMode” category  
//       – Right-click “JellyfishUnitTests” → Run Selected  
//   • Via CLI (runs only JellyfishUnitTests):
//       Unity -batchmode -projectPath . -runTests -testPlatform EditMode \
//         -testFilter JellyfishUnitTests -logFile -testResults TestResults/JellyfishUnitTests.xml

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
        jellyObj = new GameObject("Jellyfish");
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
        Assert.AreEqual(60f, jelly.stunDuration, "stunDuration must default to 60f");
        Assert.AreEqual(20, jelly.health, "health must default to 20");
    }

    [Test]
    public void GetRandomDirection_IsUnitLength()
    {
        MethodInfo changeDir = typeof(Jellyfish)
            .GetMethod("ChangeDirection", BindingFlags.NonPublic | BindingFlags.Instance);
        // run ChangeDirection to set moveDirection
        changeDir.Invoke(jelly, null);

        FieldInfo dirField = typeof(Jellyfish)
            .GetField("moveDirection", BindingFlags.NonPublic | BindingFlags.Instance);
        Vector2 dir = (Vector2)dirField.GetValue(jelly);

        Assert.AreEqual(1f, dir.magnitude, 1e-3f, "moveDirection should be normalized");
    }

    [Test]
    public void TakeDamage_ReducesHealth()
    {
        int before = jelly.health;
        jelly.TakeDamage(5);
        Assert.AreEqual(before - 5, jelly.health, "TakeDamage should subtract from health");
    }
}
