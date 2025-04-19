// File: Assets/Tests/editmodetest/LionfishUnitTests.cs
// To run these EditMode tests:
//   In the Unity Editor: Window → General → Test Runner → select “EditMode” and click Run All
//   Via CLI:
//     Unity -batchmode -projectPath . -runTests -testPlatform EditMode \
//       -logFile -testResults TestResults/EditModeResults.xml

using System.Reflection;
using NUnit.Framework;
using UnityEngine;

[TestFixture]
public class LionfishUnitTests
{
    private GameObject fishObj;
    private Lionfish fish;

    [SetUp]
    public void SetUp()
    {
        fishObj = new GameObject();
        fish = fishObj.AddComponent<Lionfish>();
        fishObj.AddComponent<Rigidbody2D>();
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(fishObj);
    }

    [Test]
    public void DefaultValues_AreCorrect()
    {
        Assert.AreEqual(2f, fish.patrolSpeed, "patrolSpeed must default to 2f");
        Assert.AreEqual(3f, fish.directionChangeInterval, "directionChangeInterval must default to 3f");
        Assert.AreEqual(40, fish.health, "health must default to 40");
    }

    [Test]
    public void TakeDamage_ReducesHealth()
    {
        int before = fish.health;
        fish.TakeDamage(15);
        Assert.AreEqual(before - 15, fish.health);
    }

    [Test]
    public void ChangeDirection_SetsNormalizedPatrolDirection()
    {
        // Call private method via reflection
        MethodInfo changeDir = typeof(Lionfish).GetMethod("ChangeDirection", BindingFlags.NonPublic | BindingFlags.Instance);
        changeDir.Invoke(fish, null);

        // Retrieve private field patrolDirection
        FieldInfo dirField = typeof(Lionfish).GetField("patrolDirection", BindingFlags.NonPublic | BindingFlags.Instance);
        Vector2 dir = (Vector2)dirField.GetValue(fish);

        Assert.AreEqual(1f, dir.magnitude, 1e-3f, "patrolDirection should be a unit vector after ChangeDirection");
    }
}
