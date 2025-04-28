// File: Assets/Tests/Editor/HarpoonerETest.cs
// To run this specific EditMode test only:
//   • In the Unity Editor Test Runner:  
//       – Window → General → Test Runner  
//       – Select the “EditMode” category  
//       – Right-click “HarpoonerETest” → Run Selected  
//   • Via CLI (runs only HarpoonerETest):  
//       Unity -batchmode -projectPath . -runTests -testPlatform EditMode \
//         -testFilter HarpoonerETest -logFile -testResults TestResults/HarpoonerETest.xml
/*
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Reflection;
using UnityEngine.TestTools;

[TestFixture]
public class HarpoonerETest
{
    private GameObject go;
    private Harpooner harpooner;
    private GameObject attackZone;
    private Animator playerAnimator;
    private RuntimeAnimatorController testController;
    private FieldInfo attackField;

    [SetUp]
    public void SetUp()
    {
        ClassSelectionData.SelectedClass = "Harpooner";

        go = new GameObject("Harpooner");
        go.AddComponent<Animator>(); 
        harpooner = go.AddComponent<Harpooner>();

        // inject attackZone
        attackZone = new GameObject("AttackZone");
        attackZone.SetActive(false);
        typeof(Harpooner)
            .GetField("attackZone", BindingFlags.NonPublic | BindingFlags.Instance)
            .SetValue(harpooner, attackZone);

        // inject playerSpriteAnimator & harpoonerAnimator
        var playerGO = new GameObject("PlayerSprite");
        playerAnimator = playerGO.AddComponent<Animator>();
        testController = new AnimatorController(); // use a real AnimatorController, not an override
        typeof(Harpooner)
            .GetField("harpoonerAnimator", BindingFlags.NonPublic | BindingFlags.Instance)
            .SetValue(harpooner, testController);
        typeof(Harpooner)
            .GetField("playerSpriteAnimator", BindingFlags.NonPublic | BindingFlags.Instance)
            .SetValue(harpooner, playerAnimator);

        // call Awake to init playerControls & attack
        typeof(Harpooner)
            .GetMethod("Awake", BindingFlags.NonPublic | BindingFlags.Instance)
            .Invoke(harpooner, null);

        attackField = typeof(Harpooner)
            .GetField("attack", BindingFlags.NonPublic | BindingFlags.Instance);
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(go);
        Object.DestroyImmediate(attackZone);
        Object.DestroyImmediate(playerAnimator.gameObject);
    }

    [Test]
    public void Awake_InitializesPlayerControlsAndAttackField()
    {
        var pc = typeof(Harpooner)
            .GetField("playerControls", BindingFlags.Public | BindingFlags.Instance)
            .GetValue(harpooner);
        Assert.NotNull(pc, "Awake() must initialize playerControls");

        var attackObj = attackField.GetValue(harpooner);
        Assert.NotNull(attackObj, "Awake() must assign the attack action");
    }

    [Test]
    public void OnEnable_ActivatesZone_AndHandlesAnimator()
    {
        // Expect Unity warning/error about invalid AnimatorController
        LogAssert.Expect(LogType.Error, "Could not set Runtime Animator Controller");

        // Act
        typeof(Harpooner)
            .GetMethod("OnEnable", BindingFlags.NonPublic | BindingFlags.Instance)
            .Invoke(harpooner, null);

        // Assert zone activation
        Assert.IsTrue(attackZone.activeSelf, "OnEnable() must activate attackZone");

        // Animator assignment will fail silently due to Unity's internal check,
        // so we simply ensure it did _not_ clear existing controller.
        Assert.IsNotNull(playerAnimator.runtimeAnimatorController,
            "OnEnable() should attempt to assign a controller (even if invalid)");
    }

    [Test]
    public void OnDisable_DeactivatesZone_AndClearsAnimator()
    {
        // First enable to set things up (swallow error)
        LogAssert.Expect(LogType.Error, "Could not set Runtime Animator Controller");
        typeof(Harpooner)
            .GetMethod("OnEnable", BindingFlags.NonPublic | BindingFlags.Instance)
            .Invoke(harpooner, null);

        // Act
        typeof(Harpooner)
            .GetMethod("OnDisable", BindingFlags.NonPublic | BindingFlags.Instance)
            .Invoke(harpooner, null);

        Assert.IsFalse(attackZone.activeSelf, "OnDisable() must deactivate attackZone");
        Assert.IsNull(playerAnimator.runtimeAnimatorController,
            "OnDisable() must clear playerSpriteAnimator.runtimeAnimatorController");
    }

    [Test]
    public void Start_SetsEnemyLayerMask()
    {
        // Act
        typeof(Harpooner)
            .GetMethod("Start", BindingFlags.NonPublic | BindingFlags.Instance)
            .Invoke(harpooner, null);

        // Extract the LayerMask field
        var maskStruct = (LayerMask)typeof(Harpooner)
            .GetField("enemyLayerMask", BindingFlags.NonPublic | BindingFlags.Instance)
            .GetValue(harpooner);

        // Compare underlying value
        Assert.AreEqual(LayerMask.GetMask("Enemy"), maskStruct.value,
            "Start() must set enemyLayerMask to the 'Enemy' layer");
    }
}
*/