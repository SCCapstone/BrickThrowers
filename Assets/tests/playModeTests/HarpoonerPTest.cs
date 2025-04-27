// File: Assets/Tests/PlayMode/HarpoonerPTest.cs
// To run this specific PlayMode test only:
//   • In the Unity Editor Test Runner:  
//       – Window → General → Test Runner  
//       – Select the “PlayMode” category  
//       – Right-click “HarpoonerPTest” → Run Selected  
//   • Via CLI (runs only HarpoonerPTest):  
//       Unity -batchmode -projectPath . -runTests -testPlatform PlayMode \
//         -testFilter HarpoonerPTest -logFile -testResults TestResults/HarpoonerPTest.xml

using NUnit.Framework;
using UnityEngine;
using System.Reflection;

public class HarpoonerPTest
{
    private GameObject go;
    private Harpooner harpooner;
    private GameObject attackZone;
    private Animator playerAnimator;
    private RuntimeAnimatorController testController;

    [SetUp]
    public void SetUp()
    {
        // Create the Harpooner GameObject and its Animator
        go = new GameObject("Harpooner");
        go.AddComponent<Animator>();
        harpooner = go.AddComponent<Harpooner>();

        // Create and assign the attackZone
        attackZone = new GameObject("AttackZone");
        attackZone.SetActive(false);
        typeof(Harpooner)
            .GetField("attackZone", BindingFlags.NonPublic | BindingFlags.Instance)
            .SetValue(harpooner, attackZone);

        // Create and assign the playerSpriteAnimator & harpoonerAnimator
        var playerGO = new GameObject("PlayerSprite");
        playerAnimator = playerGO.AddComponent<Animator>();
        testController = new AnimatorOverrideController();
        typeof(Harpooner)
            .GetField("harpoonerAnimator", BindingFlags.NonPublic | BindingFlags.Instance)
            .SetValue(harpooner, testController);
        typeof(Harpooner)
            .GetField("playerSpriteAnimator", BindingFlags.NonPublic | BindingFlags.Instance)
            .SetValue(harpooner, playerAnimator);
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(go);
        Object.DestroyImmediate(attackZone);
        Object.DestroyImmediate(playerAnimator.gameObject);
    }

    [Test]
    public void OnEnable_ActivatesZone_AndSetsAnimator()
    {
        // Simulate Awake and OnEnable
        typeof(Harpooner)
            .GetMethod("Awake", BindingFlags.NonPublic | BindingFlags.Instance)
            .Invoke(harpooner, null);
        typeof(Harpooner)
            .GetMethod("OnEnable", BindingFlags.NonPublic | BindingFlags.Instance)
            .Invoke(harpooner, null);

        Assert.IsTrue(attackZone.activeSelf, 
            "OnEnable() must activate the attackZone");
        Assert.AreEqual(testController, playerAnimator.runtimeAnimatorController,
            "OnEnable() must assign the harpoonerAnimator to playerSpriteAnimator");
    }

    [Test]
    public void OnDisable_DeactivatesZone_AndClearsAnimator()
    {
        // First simulate enabling
        typeof(Harpooner)
            .GetMethod("Awake", BindingFlags.NonPublic | BindingFlags.Instance)
            .Invoke(harpooner, null);
        typeof(Harpooner)
            .GetMethod("OnEnable", BindingFlags.NonPublic | BindingFlags.Instance)
            .Invoke(harpooner, null);

        // Now simulate disabling
        typeof(Harpooner)
            .GetMethod("OnDisable", BindingFlags.NonPublic | BindingFlags.Instance)
            .Invoke(harpooner, null);

        Assert.IsFalse(attackZone.activeSelf, 
            "OnDisable() must deactivate the attackZone");
        Assert.IsNull(playerAnimator.runtimeAnimatorController,
            "OnDisable() must clear playerSpriteAnimator.runtimeAnimatorController");
    }
}
