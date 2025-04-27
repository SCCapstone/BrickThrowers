// File: Assets/Tests/Editor/HarpoonerETest.cs
// To run this specific EditMode test only:
//   • In the Unity Editor Test Runner:  
//       – Window → General → Test Runner  
//       – Select the “EditMode” category  
//       – Right-click “HarpoonerETest” → Run Selected  
//   • Via CLI (runs only HarpoonerETest):  
//       Unity -batchmode -projectPath . -runTests -testPlatform EditMode \
//         -testFilter HarpoonerETest -logFile -testResults TestResults/HarpoonerETest.xml

using NUnit.Framework;
using UnityEngine;
using System.Reflection;

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
        // Create Harpooner and its Animator field
        go = new GameObject("Harpooner");
        go.AddComponent<Animator>();
        harpooner = go.AddComponent<Harpooner>();

        // Inject attackZone
        attackZone = new GameObject("AttackZone");
        attackZone.SetActive(false);
        typeof(Harpooner)
            .GetField("attackZone", BindingFlags.NonPublic | BindingFlags.Instance)
            .SetValue(harpooner, attackZone);

        // Inject harpoonerAnimator and playerSpriteAnimator
        var playerGO = new GameObject("PlayerSprite");
        playerAnimator = playerGO.AddComponent<Animator>();
        testController = new AnimatorOverrideController();
        typeof(Harpooner)
            .GetField("harpoonerAnimator", BindingFlags.NonPublic | BindingFlags.Instance)
            .SetValue(harpooner, testController);
        typeof(Harpooner)
            .GetField("playerSpriteAnimator", BindingFlags.NonPublic | BindingFlags.Instance)
            .SetValue(harpooner, playerAnimator);

        // Call Awake() to initialize playerControls and attack field
        typeof(Harpooner)
            .GetMethod("Awake", BindingFlags.NonPublic | BindingFlags.Instance)
            .Invoke(harpooner, null);

        // Cache the private attack field for later checks
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
        // playerControls should be non-null
        var pc = typeof(Harpooner)
            .GetField("playerControls", BindingFlags.Public | BindingFlags.Instance)
            .GetValue(harpooner);
        Assert.NotNull(pc, "Awake() must initialize playerControls");

        // attack field should be non-null
        var attackObj = attackField.GetValue(harpooner);
        Assert.NotNull(attackObj, "Awake() must assign the attack action");
    }

    [Test]
    public void OnEnable_ActivatesZoneAndAssignsAnimator()
    {
        // Preconditions
        attackZone.SetActive(false);
        playerAnimator.runtimeAnimatorController = null;

        // Act
        typeof(Harpooner)
            .GetMethod("OnEnable", BindingFlags.NonPublic | BindingFlags.Instance)
            .Invoke(harpooner, null);

        // Assert
        Assert.IsTrue(attackZone.activeSelf, 
            "OnEnable() must activate attackZone");
        Assert.AreEqual(testController, playerAnimator.runtimeAnimatorController,
            "OnEnable() must set playerSpriteAnimator.runtimeAnimatorController");
    }

    [Test]
    public void OnDisable_DeactivatesZoneAndClearsAnimator()
    {
        // First enable
        typeof(Harpooner)
            .GetMethod("OnEnable", BindingFlags.NonPublic | BindingFlags.Instance)
            .Invoke(harpooner, null);
        Assert.IsTrue(attackZone.activeSelf);

        // Act
        typeof(Harpooner)
            .GetMethod("OnDisable", BindingFlags.NonPublic | BindingFlags.Instance)
            .Invoke(harpooner, null);

        // Assert
        Assert.IsFalse(attackZone.activeSelf, 
            "OnDisable() must deactivate attackZone");
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

        // Assert
        var mask = (LayerMask)typeof(Harpooner)
            .GetField("enemyLayerMask", BindingFlags.NonPublic | BindingFlags.Instance)
            .GetValue(harpooner);
        Assert.AreEqual(LayerMask.GetMask("Enemy"), mask,
            "Start() must set enemyLayerMask to the 'Enemy' layer");
    }
}
