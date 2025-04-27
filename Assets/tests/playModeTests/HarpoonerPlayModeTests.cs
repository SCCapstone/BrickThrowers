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
    private GameObject harpoonerObj;
    private Harpooner harpooner;
    private GameObject attackPoint;
    private GameObject enemy;
    private Rigidbody2D enemyRb;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        ClassSelectionData.SelectedClass = "Harpooner";

        // Create Harpooner game object and add required components
        harpoonerObj = new GameObject("Harpooner");
        harpoonerObj.AddComponent<Animator>();
        harpoonerObj.AddComponent<Rigidbody2D>();
        harpooner = harpoonerObj.AddComponent<Harpooner>();

        // Create and assign the attack point (must match Start logic)
        attackPoint = new GameObject("AttackPoint");
        attackPoint.transform.parent = harpoonerObj.transform;
        harpooner.attackPoint = attackPoint.transform;

        // Wait a frame to let Start() run and attach the collider
        yield return null;

        // Ensure the collider is present
        var col = attackPoint.GetComponent<CircleCollider2D>();
        Assert.IsNotNull(col, "Start should have added a CircleCollider2D");

        // Create an enemy within range
        enemy = new GameObject("Enemy");
        enemy.tag = "Enemy";
        enemyRb = enemy.AddComponent<Rigidbody2D>();
        enemy.transform.position = attackPoint.transform.position
                                  + Vector3.right * (harpooner.attackRange * 0.5f);

        yield return null; // let physics settle
    }

    [UnityTearDown]
    public IEnumerator TearDown()
    {
        Object.Destroy(harpoonerObj);
        Object.Destroy(enemy);
        yield return null;
    }

    [UnityTest]
    public IEnumerator HandleAttack_AppliesKnockbackToEnemy()
    {
        // Invoke the private HandleAttack method directly
        var handle = typeof(Harpooner)
            .GetMethod("HandleAttack", BindingFlags.NonPublic | BindingFlags.Instance);
        handle.Invoke(harpooner, null);

        // Wait for physics to process the impulse
        yield return new WaitForFixedUpdate();

        Assert.Greater(enemyRb.velocity.magnitude, 0f,
            "Enemy Rigidbody should receive a knockback impulse when HandleAttack is called");
    }
}
