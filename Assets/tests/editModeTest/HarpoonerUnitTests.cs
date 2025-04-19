// File: Assets/Tests/editmodetest/HarpoonerUnitTests.cs
// To run these EditMode tests:
//   In the Unity Editor: Window → General → Test Runner → select “EditMode” and click Run All
//   Via CLI:
//     Unity -batchmode -projectPath . -runTests -testPlatform EditMode \
//       -logFile -testResults TestResults/EditModeResults.xml

using NUnit.Framework;
using UnityEngine;
using System.Reflection;

[TestFixture]
public class HarpoonerUnitTests
{
    private GameObject obj;
    private Harpooner harpooner;
    private GameObject attackPoint;

    [SetUp]
    public void SetUp()
    {
        obj = new GameObject("HarpoonerObj");
        harpooner = obj.AddComponent<Harpooner>();

        attackPoint = new GameObject("AttackPoint");
        attackPoint.transform.parent = obj.transform;
        harpooner.attackPoint = attackPoint.transform;
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(obj);
    }

    [Test]
    public void Start_CreatesAndConfiguresAttackCollider_WhenMissing()
    {
        Assert.IsNull(attackPoint.GetComponent<CircleCollider2D>());

        // Invoke Start()
        MethodInfo startMethod = typeof(Harpooner)
            .GetMethod("Start", BindingFlags.NonPublic | BindingFlags.Instance);
        startMethod.Invoke(harpooner, null);

        var collider = attackPoint.GetComponent<CircleCollider2D>();
        Assert.IsNotNull(collider, "Start should add a CircleCollider2D if none exists");
        Assert.AreEqual(harpooner.attackRange, collider.radius, 
            "Collider radius must match attackRange");
        Assert.IsTrue(collider.isTrigger, "Collider must be set as a trigger");
    }

    [Test]
    public void Start_UsesExistingAttackCollider_WhenPresent()
    {
        var existing = attackPoint.AddComponent<CircleCollider2D>();
        existing.radius = 0.2f;
        existing.isTrigger = false;

        MethodInfo startMethod = typeof(Harpooner)
            .GetMethod("Start", BindingFlags.NonPublic | BindingFlags.Instance);
        startMethod.Invoke(harpooner, null);

        var collider = attackPoint.GetComponent<CircleCollider2D>();
        Assert.AreSame(existing, collider, 
            "Start should not replace an existing CircleCollider2D");
        Assert.AreEqual(harpooner.attackRange, collider.radius, 
            "Start should overwrite radius to match attackRange");
        Assert.IsTrue(collider.isTrigger, 
            "Start should set existing collider to be a trigger");
    }
}
