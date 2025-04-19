// File: Assets/Tests/playmodetest/HarpoonerPlayModeTests.cs
// To run these PlayMode tests:
//   In the Unity Editor: Window → General → Test Runner → select “PlayMode” and click Run All
//   Via CLI:
//     Unity -batchmode -projectPath . -runTests -testPlatform PlayMode \
//       -logFile -testResults TestResults/PlayModeResults.xml

using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Reflection;

public class HarpoonerPlayModeTests
{
    private GameObject obj;
    private Harpooner harpooner;
    private GameObject attackPoint;
    private GameObject enemyObj;
    private Rigidbody2D enemyRb;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        // Ensure the Harpooner class is active
        ClassSelectionData.SelectedClass = "Harpooner";

        obj = new GameObject("Harpooner");
        harpooner = obj.AddComponent<Harpooner>();
        obj.AddComponent<Rigidbody2D>();

        attackPoint = new GameObject("AttackPoint");
        attackPoint.transform.parent = obj.transform;
        harpooner.attackPoint = attackPoint.transform;

        var collider = attackPoint.AddComponent<CircleCollider2D>();
        collider.radius = harpooner.attackRange;
        collider.isTrigger = true;

        enemyObj = new GameObject("Enemy");
        enemyObj.tag = "Enemy";
        enemyRb = enemyObj.AddComponent<Rigidbody2D>();
        enemyObj.transform.position = attackPoint.transform.position 
                                       + Vector3.right * (harpooner.attackRange * 0.5f);

        yield return null; // allow Start() to run
    }

    [UnityTearDown]
    public IEnumerator TearDown()
    {
        Object.Destroy(obj);
        Object.Destroy(enemyObj);
        yield return null;
    }

    [UnityTest]
    public IEnumerator HandleAttack_AppliesKnockbackToEnemy()
    {
        // Invoke the private HandleAttack() method
        MethodInfo attackMethod = typeof(Harpooner)
            .GetMethod("HandleAttack", BindingFlags.NonPublic | BindingFlags.Instance);
        attackMethod.Invoke(harpooner, null);

        // Wait for physics to process the impulse
        yield return new WaitForFixedUpdate();

        Assert.Greater(enemyRb.velocity.magnitude, 0f, 
            "Enemy Rigidbody should receive a knockback impulse");
    }
}
