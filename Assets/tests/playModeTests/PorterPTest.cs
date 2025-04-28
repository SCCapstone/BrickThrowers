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

    const int MAX_SLOTS = 4;
    const int DEFAULT_SLOTS = 3;
    const float EXTRA_SLOT_WIDTH = 95f;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        // Create Porter component
        porterObj = new GameObject("PorterObj");
        porter    = porterObj.AddComponent<Porter>();

        // Prepare slot prefab and holder
        slotPrefab = new GameObject("SlotPrefab");
        slotHolder = new GameObject("SlotHolder", typeof(RectTransform));
        grid       = slotHolder.AddComponent<GridLayoutGroup>();
        holderRect = slotHolder.GetComponent<RectTransform>();
        grid.constraintCount = DEFAULT_SLOTS;
        holderRect.sizeDelta = Vector2.zero;

        // Inject slot and slotHolder
        var t = typeof(Porter);
        t.GetField("slot", BindingFlags.NonPublic|BindingFlags.Instance)
            .SetValue(porter, slotPrefab);
        t.GetField("slotHolder", BindingFlags.NonPublic|BindingFlags.Instance)
            .SetValue(porter, slotHolder);

        // Inject playerSpriteAnimator only (leave porterAnimator null)
        var playerGO    = new GameObject("PlayerSprite");
        playerAnimator  = playerGO.AddComponent<Animator>();
        t.GetField("playerSpriteAnimator", BindingFlags.NonPublic|BindingFlags.Instance)
            .SetValue(porter, playerAnimator);

        // Let OnEnable run
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
    public IEnumerator OnEnable_ConfiguresInventory()
    {
        yield return null; // ensure OnEnable completed

        Assert.AreEqual(MAX_SLOTS, grid.constraintCount,
            "constraintCount should be set to MAX_SLOTS");
        Assert.AreEqual(EXTRA_SLOT_WIDTH, holderRect.sizeDelta.x, 1e-3f,
            "holder width should increase by EXTRA_SLOT_WIDTH");
        Assert.AreEqual(1, slotHolder.transform.childCount,
            "A slot child should have been added");
    }

    [UnityTest]
    public IEnumerator OnDisable_ResetsInventory()
    {
        // Disable to trigger OnDisable
        porter.enabled = false;
        yield return null; // wait for ResetPorterInventory

        Assert.AreEqual(DEFAULT_SLOTS, grid.constraintCount,
            "constraintCount should reset to DEFAULT_SLOTS");
        Assert.AreEqual(0f, holderRect.sizeDelta.x, 1e-3f,
            "holder width should reset to original");
        Assert.AreEqual(0, slotHolder.transform.childCount,
            "Slot child should be removed");
    }
}
