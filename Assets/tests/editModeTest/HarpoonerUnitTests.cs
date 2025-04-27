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
    private Animator animator;

    [SetUp]
    public void SetUp()
    {
        ClassSelectionData.SelectedClass = "Harpooner";

        // Create the harpooner object and add Animator
        obj = new GameObject("HarpoonerObj");
        animator = obj.AddComponent<Animator>();
        harpooner = obj.AddComponent<Harpooner>();

        // Create and assign the attack point
        attackPoint = new GameObject("AttackPoint");
        attackPoint.transform.parent = obj.transform;
        harpooner.attackPoint = attackPoint.transform;
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(obj);
        Object.DestroyImmediate(attackPoint);
    }

    [Test]
    public void Start_InitializesAnimator_AndAddsColliderIfMissing()
    {
        // Before Start: no collider, animator field should be null
        Assert.IsNull(attackPoint.GetComponent<CircleCollider2D>());

        // Invoke Start()
        typeof(Harpooner)
            .GetMethod("Start", BindingFlags.Instance | BindingFlags.NonPublic)
            .Invoke(harpooner, null);

        // Verify Animator was assigned from the component
        var animField = typeof(Harpooner)
            .GetField("animator", BindingFlags.Instance | BindingFlags.NonPublic);
        Assert.AreSame(animator, animField.GetValue(harpooner),
            "Start should cache the Animator component");

        // Now verify collider was added and configured
        var col = attackPoint.GetComponent<CircleCollider2D>();
        Assert.IsNotNull(col, "Start should add a CircleCollider2D if none exists");
        Assert.AreEqual(harpooner.attackRange, col.radius, "Collider radius must match attackRange");
        Assert.IsTrue(col.isTrigger, "Collider must be a trigger");
    }

    [Test]
    public void Start_UsesExistingCollider_AndUpdatesIt_AndInitializesAnimator()
    {
        // Pre-add an existing collider with bogus settings
        var existing = attackPoint.AddComponent<CircleCollider2D>();
        existing.radius = 0.2f;
        existing.isTrigger = false;

        // Invoke Start()
        typeof(Harpooner)
            .GetMethod("Start", BindingFlags.Instance | BindingFlags.NonPublic)
            .Invoke(harpooner, null);

        // Animator assignment
        var animField = typeof(Harpooner)
            .GetField("animator", BindingFlags.Instance | BindingFlags.NonPublic);
        Assert.AreSame(animator, animField.GetValue(harpooner),
            "Start should cache the Animator component");

        // Collider should not be replaced but updated
        var col = attackPoint.GetComponent<CircleCollider2D>();
        Assert.AreSame(existing, col, "Start should keep the existing collider instance");
        Assert.AreEqual(harpooner.attackRange, col.radius, "Start should overwrite radius to match attackRange");
        Assert.IsTrue(col.isTrigger, "Start should set existing collider to trigger");
    }
}
