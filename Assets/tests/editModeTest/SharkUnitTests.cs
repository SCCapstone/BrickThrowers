// File: Assets/Tests/editmodetest/SharkUnitTests.cs
// To run these EditMode tests:
//  • In the Unity Editor: Window → General → Test Runner → Select “EditMode” → Run All
//  • CLI: Unity -batchmode -projectPath . -runTests -testPlatform EditMode -logFile -testResults TestResults/EditModeResults.xml

using NUnit.Framework;
using UnityEngine;

[TestFixture]
public class SharkUnitTests
{
    GameObject sharkObj;
    Shark shark;

    [SetUp]
    public void SetUp()
    {
        sharkObj = new GameObject();
        shark = sharkObj.AddComponent<Shark>();
        sharkObj.AddComponent<Rigidbody2D>();
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(sharkObj);
    }

    [Test]
    public void GetRandomDirection_IsNormalized()
    {
        Vector2 dir = shark.GetRandomDirection();
        float length = dir.magnitude;
        Assert.AreEqual(1f, length, 1e-3f, "Random direction should be unit length");
    }

    [Test]
    public void StartCharging_SetsStateCorrectly()
    {
        shark.StartCharging();
        Assert.IsTrue(sharkObj.GetComponent<Shark>().isCharging, "Shark should be charging after StartCharging()");
        Assert.Greater(shark.chargeTimer, 0f, "chargeTimer should be initialized");
        Assert.GreaterOrEqual(shark.cooldownTimer, shark.chargeCooldown, "cooldownTimer should be reset to at least chargeCooldown");
    }

    [Test]
    public void StopCharging_StopsMovement()
    {
        // Simulate a charge in progress
        shark.StartCharging();
        shark.StopCharging();
        var rb = sharkObj.GetComponent<Rigidbody2D>();
        Assert.AreEqual(Vector2.zero, rb.velocity, "Shark velocity must be zero after StopCharging()");
        Assert.IsFalse(sharkObj.GetComponent<Shark>().isCharging, "isCharging must be false after StopCharging()");
    }
}
