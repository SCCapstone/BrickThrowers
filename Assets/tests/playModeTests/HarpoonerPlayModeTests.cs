// File: Assets/Tests/playmodetest/HarpoonerPlayModeTests.cs
// To run this specific PlayMode test only:
//   • In the Unity Editor Test Runner:  
//       – Open Window → General → Test Runner  
//       – Select the “PlayMode” category  
//       – Find and right-click “HarpoonerPlayModeTests” → Run Selected  
//   • Via CLI (runs only HarpoonerPlayModeTests):  
//       Unity -batchmode -projectPath . -runTests -testPlatform PlayMode \
//         -testFilter HarpoonerPlayModeTests -logFile -testResults TestResults/HarpoonerPlayModeTests.xml

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
    private GameObject enemy;
    private Rigidbody2D enemyRb;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        ClassSelectionData.SelectedClass = "Harpooner";

        obj = new GameObject("Harpooner");
        harpooner = obj.AddComponent<Harpooner>();
        obj.AddComponent<Rigidbody2D>();

        attackPoint = new GameObject("AttackPoint");
        attackPoint.transform.parent = obj.transform;
        harpooner.attackPoint = attackPoint.transform;
        var col = attackPoint.AddComponent<CircleCollider2D>();
        col.radius = harpooner.attackRange;
        col.isTrigger = true;

        enemy = new GameObject("Enemy");
        enemy.tag = "Enemy";
        enemyRb = enemy.AddComponent<Rigidbody2D>();
        enemy.transform.position = attackPoint.transform.position + Vector3.right * (harpooner.attackRange * 0.5f);

        yield return null;
    }

    [UnityTearDown]
    public IEnumerator TearDown()
    {
        Object.Destroy(obj);
        Object.Destroy(enemy);
        yield return null;
    }

    [UnityTest]
    public IEnumerator HandleAttack_AppliesKnockbackToEnemy()
    {
        MethodInfo handle = typeof(Harpooner)
            .GetMethod("HandleAttack", BindingFlags.NonPublic | BindingFlags.Instance);
        handle.Invoke(harpooner, null);

        yield return new WaitForFixedUpdate();

        Assert.Greater(enemyRb.velocity.magnitude, 0f, "Enemy should be knocked back on attack");
    }
}
