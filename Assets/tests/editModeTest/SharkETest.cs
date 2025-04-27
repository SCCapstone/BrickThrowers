// File: Assets/Tests/Editor/SharkETest.cs
// To run this specific EditMode test only:
//   • In the Unity Editor Test Runner:
//       – Window → General → Test Runner  
//       – Select “EditMode” category  
//       – Right-click “SharkETest” → Run Selected  
//   • Via CLI (runs only SharkETest):  
//       Unity -batchmode -projectPath . -runTests -testPlatform EditMode \  
//         -testFilter SharkETest -logFile -testResults TestResults/SharkETest.xml

using System.Collections;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;

[TestFixture]
public class SharkETest
{
    private GameObject sharkObj;
    private Shark shark;
    private Rigidbody2D rb;

    [SetUp]
    public void SetUp()
    {
        sharkObj = new GameObject("Shark");
        shark = sharkObj.AddComponent<Shark>();
        rb = sharkObj.AddComponent<Rigidbody2D>();

        // Inject the Rigidbody2D into the private rb field
        typeof(Shark)
            .GetField("rb", BindingFlags.NonPublic | BindingFlags.Instance)
            .SetValue(shark, rb);
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(sharkObj);
    }

    [Test]
    public void DefaultValues_AreCorrect()
    {
        Assert.AreEqual(3f,  shark.patrolSpeed);
        Assert.AreEqual(7f,  shark.chargeSpeed);
        Assert.AreEqual(3f,  shark.chargeCooldown);
        Assert.AreEqual(10f, shark.detectionRange);
        Assert.AreEqual(10f, shark.chargeDuration);
        Assert.AreEqual(20,  shark.oxygenDamage);
        Assert.AreEqual(2f,  shark.roamDuration);
        Assert.IsFalse(shark.isCharging);
        Assert.IsFalse(shark.isPatrolling);
    }

    [Test]
    public void GetRandomDirection_IsUnitLength()
    {
        var randDir = typeof(Shark)
            .GetMethod("GetRandomDirection", BindingFlags.NonPublic | BindingFlags.Instance);
        Vector2 dir = (Vector2)randDir.Invoke(shark, null);
        Assert.AreEqual(1f, dir.magnitude, 1e-3f);
    }

    [Test]
    public void PatrolCoroutine_SetsIsPatrollingTrueAndVelocity()
    {
        var patrolMethod = typeof(Shark)
            .GetMethod("Patrol", BindingFlags.NonPublic | BindingFlags.Instance);
        var enumerator = (IEnumerator)patrolMethod.Invoke(shark, null);

        // Advance to first yield
        Assert.IsTrue(enumerator.MoveNext(), "Patrol coroutine should yield once");

        // isPatrolling should be set
        Assert.IsTrue(shark.isPatrolling, "isPatrolling should be true when Patrol starts");

        // Rigidbody should have been driven at patrolSpeed
        float speed = rb.velocity.magnitude;
        Assert.AreEqual(shark.patrolSpeed, speed, 1e-3f, "Patrol should set velocity to patrolSpeed");
    }

    [Test]
    public void Charge_SetsStateAndTimers()
    {
        var chargeMethod = typeof(Shark)
            .GetMethod("Charge", BindingFlags.NonPublic | BindingFlags.Instance);
        chargeMethod.Invoke(shark, null);

        Assert.IsTrue(shark.isCharging, "Charge() should set isCharging true");

        float chargeTimer = (float)typeof(Shark)
            .GetField("chargeTimer", BindingFlags.NonPublic | BindingFlags.Instance)
            .GetValue(shark);
        float cooldownTimer = (float)typeof(Shark)
            .GetField("cooldownTimer", BindingFlags.NonPublic | BindingFlags.Instance)
            .GetValue(shark);

        Assert.AreEqual(shark.chargeDuration, chargeTimer);
        Assert.AreEqual(shark.chargeCooldown, cooldownTimer);
    }

    [Test]
    public void StopCharging_ResetsStateAndVelocity()
    {
        // Simulate a charging state with non-zero velocity
        shark.isCharging = true;
        rb.velocity = new Vector2(5f, 0f);

        var stop = typeof(Shark)
            .GetMethod("StopCharging", BindingFlags.NonPublic | BindingFlags.Instance);
        stop.Invoke(shark, null);

        Assert.IsFalse(shark.isCharging, "StopCharging() should set isCharging false");
        Assert.AreEqual(Vector2.zero, rb.velocity, "StopCharging() should zero out velocity");
    }
}
