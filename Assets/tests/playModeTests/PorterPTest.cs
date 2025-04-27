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
    private GameObject porterObj;
    private Porter porter;
    private GameObject slotPrefab, slotHolder;
    private GridLayoutGroup grid;
    private RectTransform holderRect;
    private Animator playerAnimator;
    private RuntimeAnimatorController testController;

    const int MAX_SLOTS = 4, DEFAULT_SLOTS = 3;
    const float EXTRA_SLOT_WIDTH = 95f;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        // Create inactive GameObject so OnEnable won't fire immediately
        porterObj = new GameObject("PorterObj");
        porterObj.SetActive(false);
        porter = porterObj.AddComponent<Porter>();

        // Prepare dependencies
        slotPrefab = new GameObject("SlotPrefab");
        slotHolder = new GameObject("SlotHolder", typeof(RectTransform));
        grid       = slotHolder.AddComponent<GridLayoutGroup>();
        holderRect = slotHolder.GetComponent<RectTransform>();
        grid.constraintCount = DEFAULT_SLOTS;
        holderRect.sizeDelta = Vector2.zero;

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

        // Expect the null-ref in SetPorterInventory and the warning about AnimatorOverrideController
        LogAssert.Expect(LogType.Exception, "NullReferenceException");
        LogAssert.Expect(LogType.Error, "Could not set Runtime Animator Controller");

        // Now activate to trigger OnEnable
        porterObj.SetActive(true);
        yield return null; // allow OnEnable to run and be caught by LogAssert
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
        // After one frame, inventory setup should have been attempted
        Assert.AreEqual(MAX_SLOTS,        grid.constraintCount, "constraintCount should be MAX_SLOTS");
        Assert.AreEqual(EXTRA_SLOT_WIDTH, holderRect.sizeDelta.x, 1e-3f,    "holder width should increase");
        Assert.AreEqual(1,                slotHolder.transform.childCount, "One slot should be added");
        Assert.AreEqual(testController,   playerAnimator.runtimeAnimatorController,
            "playerSpriteAnimator should be set to porterAnimator");
        yield break;
    }

    [UnityTest]
    public IEnumerator OnDisable_ResetsInventoryAndAnimator()
    {
        // Disable the component to trigger OnDisable
        porter.enabled = false;
        yield return null; // allow ResetPorterInventory to complete

        Assert.AreEqual(DEFAULT_SLOTS, grid.constraintCount, "constraintCount should reset to DEFAULT_SLOTS");
        Assert.AreEqual(0f,            holderRect.sizeDelta.x, 1e-3f, "holder width should reset");
        Assert.AreEqual(0,             slotHolder.transform.childCount, "Slot should be removed");
        Assert.IsNull(playerAnimator.runtimeAnimatorController,
            "playerSpriteAnimator.runtimeAnimatorController should be null after disable");
        yield break;
    }
}
