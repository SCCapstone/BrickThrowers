// File: Assets/Tests/PlayMode/HarpoonerPTest.cs
// To run this specific PlayMode test only:
//   • In the Unity Editor Test Runner:
//       – Window → General → Test Runner  
//       – Select “PlayMode” category  
//       – Right-click “HarpoonerPTest” → Run Selected  
//   • Via CLI (runs only HarpoonerPTest):  
//       Unity -batchmode -projectPath . -runTests -testPlatform PlayMode \  
//         -testFilter HarpoonerPTest -logFile -testResults TestResults/HarpoonerPTest.xml

using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Reflection;

public class HarpoonerPTest
{
    private GameObject go;
    private Harpooner harpooner;
    private GameObject attackZone;
    private Animator playerAnimator;
    private RuntimeAnimatorController testController;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        // Create the GameObject but keep it inactive to inject dependencies first
        go = new GameObject("Harpooner");
        go.SetActive(false);
        go.AddComponent<Animator>(); // the private 'animator' field
        harpooner = go.AddComponent<Harpooner>();

        // Inject the attackZone
        attackZone = new GameObject("AttackZone");
        attackZone.SetActive(false);
        typeof(Harpooner)
            .GetField("attackZone", BindingFlags.NonPublic | BindingFlags.Instance)
            .SetValue(harpooner, attackZone);

        // Inject harpoonerAnimator & playerSpriteAnimator
        var playerGO = new GameObject("PlayerSprite");
        playerAnimator = playerGO.AddComponent<Animator>();
        testController = new AnimatorOverrideController();
        typeof(Harpooner)
            .GetField("harpoonerAnimator", BindingFlags.NonPublic | BindingFlags.Instance)
            .SetValue(harpooner, testController);
        typeof(Harpooner)
            .GetField("playerSpriteAnimator", BindingFlags.NonPublic | BindingFlags.Instance)
            .SetValue(harpooner, playerAnimator);

        // Now activate so Awake() and OnEnable() run with all fields set
        go.SetActive(true);
        yield return null;
    }

    [UnityTearDown]
    public IEnumerator TearDown()
    {
        Object.Destroy(go);
        Object.Destroy(attackZone);
        Object.Destroy(playerAnimator.gameObject);
        yield return null;
    }

    [UnityTest]
    public IEnumerator OnEnable_ActivatesZone_AndSetsAnimator()
    {
        // After activation, OnEnable() should have run
        Assert.IsTrue(attackZone.activeSelf,
            "OnEnable() must activate attackZone");
        Assert.AreEqual(testController, playerAnimator.runtimeAnimatorController,
            "OnEnable() must assign the harpoonerAnimator to playerSpriteAnimator");
        yield break;
    }

    [UnityTest]
    public IEnumerator OnDisable_DeactivatesZone_AndClearsAnimator()
    {
        // Disable the component to invoke OnDisable()
        harpooner.enabled = false;
        yield return null;

        Assert.IsFalse(attackZone.activeSelf,
            "OnDisable() must deactivate attackZone");
        Assert.IsNull(playerAnimator.runtimeAnimatorController,
            "OnDisable() must clear playerSpriteAnimator.runtimeAnimatorController");
        yield break;
    }
}
