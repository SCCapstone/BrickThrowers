// File: Assets/Tests/Editor/AnglerFishETest.cs
// To run this specific EditMode test only:
//   • In the Unity Editor Test Runner:
//       – Window → General → Test Runner  
//       – Select the “EditMode” category  
//       – Right-click “AnglerFishETest” → Run Selected  
//   • Via CLI (runs only AnglerFishETest):  
//       Unity -batchmode -projectPath . -runTests -testPlatform EditMode \  
//         -testFilter AnglerFishETest -logFile -testResults TestResults/AnglerFishETest.xml

using NUnit.Framework;
using UnityEngine;
using System.Reflection;

[TestFixture]
public class AnglerFishETest
{
    [Test]
    public void DefaultValuesAreCorrect()
    {
        var go = new GameObject("Anglerfish");
        var fish = go.AddComponent<Anglerfish>();

        Assert.AreEqual(2f,    fish.swimSpeed,                "swimSpeed should default to 2f");
        Assert.AreEqual(5f,    fish.directionChangeInterval,  "directionChangeInterval should default to 5f");
        Assert.AreEqual(3f,    fish.lightIntensity,          "lightIntensity should default to 3f");
        Assert.AreEqual(5f,    fish.detectionRange,          "detectionRange should default to 5f");
        Assert.AreEqual(0.2f,  fish.oxygenDamage,            "oxygenDamage should default to 0.2f");
        Assert.AreEqual(30,    fish.health,                  "health should default to 30");

        Object.DestroyImmediate(go);
    }

    [Test]
    public void SwimMovementOccurs_AfterStartAndUpdate()
    {
        // Arrange
        var go = new GameObject("Anglerfish");
        var rb = go.AddComponent<Rigidbody2D>();
        var fish = go.AddComponent<Anglerfish>();
        // No Light assigned => Start won't schedule flicker
        // No Player with tag => targetPlayer remains null

        // Act: call Start() then Update()
        typeof(Anglerfish)
            .GetMethod("Start", BindingFlags.Instance | BindingFlags.NonPublic)
            .Invoke(fish, null);
        typeof(Anglerfish)
            .GetMethod("Update", BindingFlags.Instance | BindingFlags.NonPublic)
            .Invoke(fish, null);

        // Assert
        float speed = rb.velocity.magnitude;
        Assert.AreEqual(fish.swimSpeed, speed, 0.1f,
            "After Start() + Update(), rb.velocity magnitude should equal swimSpeed");

        Object.DestroyImmediate(go);
    }
}