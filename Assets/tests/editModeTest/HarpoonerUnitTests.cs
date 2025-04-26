// File: Assets/Tests/editmodetest/HarpoonerUnitTests.cs
// To run this specific EditMode test only:
//   • In the Unity Editor Test Runner:  
//       – Open Window → General → Test Runner  
//       – Select the “EditMode” category  
//       – Find and right-click “HarpoonerUnitTests” → Run Selected  
//   • Via CLI (runs only HarpoonerUnitTests):  
//       Unity -batchmode -projectPath . -runTests -testPlatform EditMode \
//         -testFilter HarpoonerUnitTests -logFile -testResults TestResults/HarpoonerUnitTests.xml

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
        ClassSelectionData.SelectedClass = "Harpooner";
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
    public void Start_AddsColliderIfMissing_AndConfiguresIt()
    {
        Assert.IsNull(attackPoint.GetComponent<CircleCollider2D>());

        MethodInfo start = typeof(Harpooner)
            .GetMethod("Start", BindingFlags.NonPublic | BindingFlags.Instance);
        start.Invoke(harpooner, null);

        var col = attackPoint.GetComponent<CircleCollider2D>();
        Assert.IsNotNull(col, "Start should add a CircleCollider2D if none exists");
        Assert.AreEqual(harpooner.attackRange, col.radius, "Collider radius must match attackRange");
        Assert.IsTrue(col.isTrigger, "Collider must be a trigger");
    }

    [Test]
    public void Start_UsesExistingCollider_AndUpdatesIt()
    {
        var existing = attackPoint.AddComponent<CircleCollider2D>();
        existing.radius = 0.2f;
        existing.isTrigger = false;

        MethodInfo start = typeof(Harpooner)
            .GetMethod("Start", BindingFlags.NonPublic | BindingFlags.Instance);
        start.Invoke(harpooner, null);

        var col = attackPoint.GetComponent<CircleCollider2D>();
        Assert.AreSame(existing, col, "Should keep the existing collider instance");
        Assert.AreEqual(harpooner.attackRange, col.radius, "Should overwrite radius");
        Assert.IsTrue(col.isTrigger, "Should set existing collider to trigger");
    }
}
