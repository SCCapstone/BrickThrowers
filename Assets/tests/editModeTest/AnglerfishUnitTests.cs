// File: Assets/Tests/editmodetest/AnglerfishUnitTests.cs
// To run this specific EditMode test only:
//   • In the Unity Editor Test Runner:
//       – Window → General → Test Runner
//       – Select “EditMode” category
//       – Find and right-click “AnglerfishUnitTests” → Run Selected
//   • Via CLI (runs only AnglerfishUnitTests):
//       Unity -batchmode -projectPath . -runTests -testPlatform EditMode \
//         -testFilter AnglerfishUnitTests -logFile -testResults TestResults/AnglerfishUnitTests.xml

using NUnit.Framework;
using UnityEngine;
using System.Reflection;

[TestFixture]
public class AnglerfishUnitTests
{
    private GameObject fishObj;
    private Anglerfish fish;

    [SetUp]
    public void SetUp()
    {
        fishObj = new GameObject("Anglerfish");
        fish = fishObj.AddComponent<Anglerfish>();
        fishObj.AddComponent<Rigidbody2D>();
        var light = fishObj.AddComponent<Light>();  // ensure Start() runs past initialization
        fish.anglerLight = light;
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(fishObj);
    }

    [Test]
    public void DefaultValues_AreCorrect()
    {
        Assert.AreEqual(2f, fish.swimSpeed);
        Assert.AreEqual(5f, fish.directionChangeInterval);
        Assert.AreEqual(3f, fish.lightIntensity);
        Assert.AreEqual(0.1f, fish.flickerFrequency);
        Assert.AreEqual(5f, fish.detectionRange);
        Assert.AreEqual(0.2f, fish.oxygenDamage, 1e-6f);
        Assert.AreEqual(30, fish.health);
    }

    [Test]
    public void GetRandomDirection_IsUnitLength()
    {
        MethodInfo randDir = typeof(Anglerfish)
            .GetMethod("GetRandomDirection", BindingFlags.NonPublic | BindingFlags.Instance);
        Vector2 dir = (Vector2)randDir.Invoke(fish, null);
        Assert.AreEqual(1f, dir.magnitude, 1e-3f);
    }

    [Test]
    public void FlickerLight_IntensityWithinRange()
    {
        fish.lightIntensity = 4f;
        fish.FlickerLight();
        float intensity = fish.anglerLight.intensity;
        Assert.GreaterOrEqual(intensity, 0.5f);
        Assert.LessOrEqual(intensity, fish.lightIntensity);
    }

    [Test]
    public void TakeDamage_ReducesHealth()
    {
        int before = fish.health;
        fish.TakeDamage(10);
        Assert.AreEqual(before - 10, fish.health);
    }
}
