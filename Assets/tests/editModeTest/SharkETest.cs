using NUnit.Framework;
using UnityEngine;
using System.Collections;
using System.Reflection;

[TestFixture]
public class SharkETest
{
    private GameObject sharkObj;
    private Shark shark;
    private Rigidbody2D rb;
    private GameObject dummy;

    [SetUp]
    public void SetUp()
    {
        // Create Shark and its Rigidbody2D
        sharkObj = new GameObject("Shark");
        shark    = sharkObj.AddComponent<Shark>();
        rb       = sharkObj.AddComponent<Rigidbody2D>();

        // Inject private rb field
        typeof(Shark)
            .GetField("rb", BindingFlags.NonPublic | BindingFlags.Instance)
            .SetValue(shark, rb);

        // Provide a dummy targetPlayer so Charge() won't NRE
        dummy = new GameObject("Player");
        typeof(Shark)
            .GetField("targetPlayer", BindingFlags.NonPublic | BindingFlags.Instance)
            .SetValue(shark, dummy.transform);
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(sharkObj);
        Object.DestroyImmediate(dummy);
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
        var dir = (Vector2)typeof(Shark)
            .GetMethod("GetRandomDirection", BindingFlags.NonPublic | BindingFlags.Instance)
            .Invoke(shark, null);
        Assert.AreEqual(1f, dir.magnitude, 1e-3f);
    }

    [Test]
    public void PatrolCoroutine_SetsIsPatrollingTrueAndVelocity()
    {
        var enumerator = (IEnumerator)typeof(Shark)
            .GetMethod("Patrol", BindingFlags.NonPublic | BindingFlags.Instance)
            .Invoke(shark, null);

        Assert.IsTrue(enumerator.MoveNext(), "Patrol coroutine should yield once");
        Assert.IsTrue(shark.isPatrolling, "isPatrolling should be true at start of Patrol");
        Assert.AreEqual(shark.patrolSpeed, rb.velocity.magnitude, 1e-3f,
            "Patrol should set rb.velocity to patrolSpeed");
    }

    [Test]
    public void Charge_SetsStateAndTimers()
    {
        typeof(Shark)
            .GetMethod("Charge", BindingFlags.NonPublic | BindingFlags.Instance)
            .Invoke(shark, null);

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
        shark.isCharging = true;
        rb.velocity = Vector2.one;

        typeof(Shark)
            .GetMethod("StopCharging", BindingFlags.NonPublic | BindingFlags.Instance)
            .Invoke(shark, null);

        Assert.IsFalse(shark.isCharging, "StopCharging() should set isCharging false");
        Assert.AreEqual(Vector2.zero, rb.velocity, "StopCharging() should zero out velocity");
    }
}