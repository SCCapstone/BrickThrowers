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

    [SetUp]
    public void SetUp()
    {
        ClassSelectionData.SelectedClass = "Harpooner";

        // Create Harpooner GameObject with required components
        go = new GameObject("Harpooner");
        go.AddComponent<Animator>(); // for private `animator`
        harpooner = go.AddComponent<Harpooner>();

        // Create and assign attackZone (serialized field)
        attackZone = new GameObject("AttackZone");
        attackZone.SetActive(false);
        typeof(Harpooner)
          .GetField("attackZone", BindingFlags.NonPublic | BindingFlags.Instance)
          .SetValue(harpooner, attackZone);

        // Create and assign playerSpriteAnimator & harpoonerAnimator
        var playerGO = new GameObject("Player");
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
    public void Awake_InitializesPlayerControlsAndAttackField()
    {
        // Act
        typeof(Harpooner)
          .GetMethod("Awake", BindingFlags.NonPublic | BindingFlags.Instance)
          .Invoke(harpooner, null);

        // Assert: playerControls is non-null
        var pcField = typeof(Harpooner)
          .GetField("playerControls", BindingFlags.Public | BindingFlags.Instance);
        Assert.NotNull(pcField.GetValue(harpooner), "Awake() must initialize playerControls");

        // Assert: private 'attack' field is non-null
        var attackField = typeof(Harpooner)
          .GetField("attack", BindingFlags.NonPublic | BindingFlags.Instance);
        Assert.NotNull(attackField.GetValue(harpooner), "Awake() must assign the attack action");
    }

    [Test]
    public void OnEnable_ActivatesZone_AndSetsAnimator()
    {
        // Prepare
        Awake_InitializesPlayerControlsAndAttackField();
        attackZone.SetActive(false);
        playerAnimator.runtimeAnimatorController = null;

        // Act
        typeof(Harpooner)
          .GetMethod("OnEnable", BindingFlags.NonPublic | BindingFlags.Instance)
          .Invoke(harpooner, null);

        // Assert
        Assert.IsTrue(attackZone.activeSelf, "OnEnable() must activate attackZone");
        Assert.AreEqual(testController, playerAnimator.runtimeAnimatorController,
            "OnEnable() must set playerSpriteAnimator.runtimeAnimatorController");
    }

    [Test]
    public void OnDisable_DeactivatesZone_AndClearsAnimator()
    {
        // Prepare
        Awake_InitializesPlayerControlsAndAttackField();
        typeof(Harpooner)
          .GetMethod("OnEnable", BindingFlags.NonPublic | BindingFlags.Instance)
          .Invoke(harpooner, null);
        Assert.IsTrue(attackZone.activeSelf);

        // Act
        typeof(Harpooner)
          .GetMethod("OnDisable", BindingFlags.NonPublic | BindingFlags.Instance)
          .Invoke(harpooner, null);

        // Assert
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

        // Assert
        var mask = (LayerMask)typeof(Harpooner)
          .GetField("enemyLayerMask", BindingFlags.NonPublic | BindingFlags.Instance)
          .GetValue(harpooner);
        Assert.AreEqual(LayerMask.GetMask("Enemy"), mask,
            "Start() must set enemyLayerMask to the 'Enemy' layer");
    }

}
