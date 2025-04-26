// File: Assets/Tests/editmodetest/LionfishUnitTests.cs
// To run this specific EditMode test only:
//   • In the Unity Editor Test Runner:
//       – Window → General → Test Runner  
//       – Select “EditMode” category  
//       – Right-click “LionfishUnitTests” → Run Selected  
//   • Via CLI (runs only LionfishUnitTests):
//       Unity -batchmode -projectPath . -runTests -testPlatform EditMode \
//         -testFilter LionfishUnitTests -logFile -testResults TestResults/LionfishUnitTests.xml

using NUnit.Framework;
using UnityEngine;
using System.Reflection;

[TestFixture]
public class LionfishUnitTests
{
    private GameObject fishObj;
    private Lionfish fish;

    [SetUp]
    public void SetUp()
    {
        fishObj = new GameObject("Lionfish");
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
    public void ChangeDirection_SetsNormalizedPatrolDirection()
    {
        // invoke private ChangeDirection()
        MethodInfo changeDir = typeof(Lionfish)
            .GetMethod("ChangeDirection", BindingFlags.NonPublic | BindingFlags.Instance);
        changeDir.Invoke(fish, null);

        // read private patrolDirection field
        FieldInfo dirField = typeof(Lionfish)
            .GetField("patrolDirection", BindingFlags.NonPublic | BindingFlags.Instance);
        Vector2 dir = (Vector2)dirField.GetValue(fish);

        Assert.AreEqual(1f, dir.magnitude, 1e-3f, "patrolDirection should be normalized");
    }

    [Test]
    public void TakeDamage_ReducesHealth()
    {
        int before = fish.health;
        fish.TakeDamage(15);
        Assert.AreEqual(before - 15, fish.health, "TakeDamage should subtract from health");
    }
}
