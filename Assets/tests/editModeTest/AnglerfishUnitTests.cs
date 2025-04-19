// File: Assets/Tests/editmodetest/AnglerfishUnitTests.cs
// To run these EditMode tests:
//   In the Unity Editor: open Window → General → Test Runner, select “EditMode” and click Run All
//   Via CLI:
//     Unity -batchmode -projectPath . -runTests -testPlatform EditMode \
//       -logFile -testResults TestResults/EditModeResults.xml

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
        fishObj = new GameObject();
        fish = fishObj.AddComponent<Anglerfish>();
        fishObj.AddComponent<Rigidbody2D>();

        // create and assign a Light for flicker tests
        var lightObj = new GameObject("Light");
        var light = lightObj.AddComponent<Light>();
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
        Assert.AreEqual(0.1f, fish.oxygenDamage);
    }

    [Test]
    public void GetRandomDirection_IsNormalized()
    {
        MethodInfo randDir = typeof(Anglerfish).GetMethod("GetRandomDirection", BindingFlags.NonPublic | BindingFlags.Instance);
        Vector2 dir = (Vector2)randDir.Invoke(fish, null);
        Assert.AreEqual(1f, dir.magnitude, 1e-3f);
    }

    [Test]
    public void FlickerLight_SetsIntensityWithinRange()
    {
        fish.lightIntensity = 4f;
        fish.FlickerLight();
        float intensity = fish.anglerLight.intensity;
        Assert.GreaterOrEqual(intensity, 0.5f);
        Assert.LessOrEqual(intensity, fish.lightIntensity);
    }

    [Test]
    public void InteractWithPlayer_ReducesOxygen()
    {
        var playerObj = new GameObject("Player");
        playerObj.tag = "Player";
        var player = playerObj.AddComponent<Player>();
        player.oxygen = 10f;

        fish.targetPlayer = playerObj.transform;
        fish.InteractWithPlayer();

        Assert.Less(player.oxygen, 10f);
    }
}
