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
        // Create GameObject and add Harpooner
        go = new GameObject("Harpooner");
        go.AddComponent<Animator>(); // to satisfy internal animator
        harpooner = go.AddComponent<Harpooner>();

        // Create and assign attackZone
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

        yield return null; // let Awake → OnEnable → Start run
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
        // After a frame, OnEnable() has run
        Assert.IsTrue(attackZone.activeSelf, "attackZone should be active after OnEnable");
        Assert.AreEqual(testController, playerAnimator.runtimeAnimatorController,
            "playerSpriteAnimator should have harpoonerAnimator assigned");
        yield break;
    }

    [UnityTest]
    public IEnumerator OnDisable_DeactivatesZone_AndClearsAnimator()
    {
        // Disable component to trigger OnDisable
        harpooner.enabled = false;
        yield return null; // wait one frame

        Assert.IsFalse(attackZone.activeSelf, "attackZone should be inactive after OnDisable");
        Assert.IsNull(playerAnimator.runtimeAnimatorController,
            "playerSpriteAnimator.runtimeAnimatorController should be null after OnDisable");
        yield break;
    }

}
