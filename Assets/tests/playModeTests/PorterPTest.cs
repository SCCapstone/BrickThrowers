// File: Assets/Tests/PlayMode/PorterPTest.cs
// To run this specific PlayMode test only:
//   • In the Unity Editor Test Runner:
//       – Window → General → Test Runner  
//       – Select “PlayMode” category  
//       – Right-click “PorterPTest” → Run Selected  
//   • Via CLI (runs only PorterPTest):  
//       Unity -batchmode -projectPath . -runTests -testPlatform PlayMode \  
//         -testFilter PorterPTest -logFile -testResults TestResults/PorterPTest.xml

using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using UnityEngine.UI;
using System.Reflection;

public class PorterPTest
{
    GameObject porterObj;
    Porter porter;
    GameObject slotPrefab, slotHolder;
    GridLayoutGroup grid;
    RectTransform holderRect;
    Animator playerAnimator;
    RuntimeAnimatorController testController;

    const int MAX_SLOTS = 4, DEFAULT_SLOTS = 3;
    const float EXTRA_SLOT_WIDTH = 95f;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        // Create the Porter and its dependencies
        porterObj = new GameObject("PorterObj");
        porter    = porterObj.AddComponent<Porter>();

        slotPrefab = new GameObject("SlotPrefab");
        slotHolder = new GameObject("SlotHolder", typeof(RectTransform));
        grid       = slotHolder.AddComponent<GridLayoutGroup>();
        holderRect = slotHolder.GetComponent<RectTransform>();

        // Initial layout
        grid.constraintCount = DEFAULT_SLOTS;
        holderRect.sizeDelta = Vector2.zero;

        // Dummy animator
        var playerGO       = new GameObject("PlayerSprite");
        playerAnimator     = playerGO.AddComponent<Animator>();
        testController     = new AnimatorOverrideController();

        // Inject serialized fields
        var t = typeof(Porter);
        t.GetField("slot",               BindingFlags.NonPublic|BindingFlags.Instance)
            .SetValue(porter, slotPrefab);
        t.GetField("slotHolder",         BindingFlags.NonPublic|BindingFlags.Instance)
            .SetValue(porter, slotHolder);
        t.GetField("porterAnimator",     BindingFlags.NonPublic|BindingFlags.Instance)
            .SetValue(porter, testController);
        t.GetField("playerSpriteAnimator", BindingFlags.NonPublic|BindingFlags.Instance)
            .SetValue(porter, playerAnimator);

        // Manually invoke OnEnable
        typeof(Porter)
            .GetMethod("OnEnable", BindingFlags.NonPublic|BindingFlags.Instance)
            .Invoke(porter, null);

        // Wait a frame for the inventory to be set up
        yield return null;
    }

    [UnityTearDown]
    public IEnumerator TearDown()
    {
        Object.Destroy(porterObj);
        Object.Destroy(slotPrefab);
        Object.Destroy(slotHolder);
        Object.Destroy(playerAnimator.gameObject);
        yield return null;
    }

    [UnityTest]
    public IEnumerator OnEnable_ConfiguresInventoryAndAnimator()
    {
        // After Setup, OnEnable() has run
        Assert.AreEqual(MAX_SLOTS,        grid.constraintCount, "constraintCount should be MAX_SLOTS");
        Assert.AreEqual(EXTRA_SLOT_WIDTH, holderRect.sizeDelta.x, 1e-3f, "holder width should increase");
        Assert.AreEqual(1,                slotHolder.transform.childCount, "One slot should be added");
        Assert.AreEqual(testController,   playerAnimator.runtimeAnimatorController,
            "playerSpriteAnimator should be set to porterAnimator");
        yield break;
    }

    [UnityTest]
    public IEnumerator OnDisable_ResetsInventoryAndAnimator()
    {
        // Manually invoke OnDisable
        typeof(Porter)
            .GetMethod("OnDisable", BindingFlags.NonPublic|BindingFlags.Instance)
            .Invoke(porter, null);

        // Wait a frame so Destroy() in ResetPorterInventory completes
        yield return null;

        Assert.AreEqual(DEFAULT_SLOTS, grid.constraintCount, "constraintCount should reset to DEFAULT_SLOTS");
        Assert.AreEqual(0f,            holderRect.sizeDelta.x, 1e-3f, "holder width should reset");
        Assert.AreEqual(0,             slotHolder.transform.childCount, "Slot should be removed");
        Assert.IsNull(playerAnimator.runtimeAnimatorController,
            "playerSpriteAnimator.runtimeAnimatorController should be null after disable");
        yield break;
    }
}
