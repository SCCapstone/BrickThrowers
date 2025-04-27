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
    private GameObject slotPrefab;
    private GameObject slotHolder;
    private GridLayoutGroup grid;
    private RectTransform holderRect;
    private Animator playerAnimator;
    private RuntimeAnimatorController testController;

    const int MAX_SLOTS = 4;
    const int DEFAULT_SLOTS = 3;
    const float EXTRA_SLOT_WIDTH = 95f;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        // Create Porter component
        porterObj = new GameObject("PorterObj");
        porter = porterObj.AddComponent<Porter>();

        // Prepare slot prefab and holder
        slotPrefab = new GameObject("SlotPrefab");
        slotHolder = new GameObject("SlotHolder", typeof(RectTransform));
        grid = slotHolder.AddComponent<GridLayoutGroup>();
        holderRect = slotHolder.GetComponent<RectTransform>();
        holderRect.sizeDelta = Vector2.zero;
        grid.constraintCount = DEFAULT_SLOTS;

        // Prepare animator
        var playerObj = new GameObject("Player");
        playerAnimator = playerObj.AddComponent<Animator>();
        testController = new AnimatorOverrideController();

        // Inject serialized fields via reflection
        var type = typeof(Porter);
        type.GetField("slot", BindingFlags.NonPublic | BindingFlags.Instance)
            .SetValue(porter, slotPrefab);
        type.GetField("slotHolder", BindingFlags.NonPublic | BindingFlags.Instance)
            .SetValue(porter, slotHolder);
        type.GetField("porterAnimator", BindingFlags.NonPublic | BindingFlags.Instance)
            .SetValue(porter, testController);
        type.GetField("playerSpriteAnimator", BindingFlags.NonPublic | BindingFlags.Instance)
            .SetValue(porter, playerAnimator);

        // Let Unity call OnEnable
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
        // After one frame, OnEnable() has run
        yield return null;

        Assert.AreEqual(MAX_SLOTS, grid.constraintCount, "constraintCount should be set to MAX_SLOTS");
        Assert.AreEqual(EXTRA_SLOT_WIDTH, holderRect.sizeDelta.x, 1e-3f, "holder width should increase by EXTRA_SLOT_WIDTH");
        Assert.AreEqual(1, slotHolder.transform.childCount, "A slot child should have been added");
        Assert.AreEqual(testController, playerAnimator.runtimeAnimatorController, "Animator should be set to porterAnimator");
    }

    [UnityTest]
    public IEnumerator OnDisable_ResetsInventoryAndAnimator()
    {
        // Disable the component to trigger OnDisable
        porter.enabled = false;

        // Wait one frame for OnDisable to apply
        yield return null;

        Assert.AreEqual(DEFAULT_SLOTS, grid.constraintCount, "constraintCount should reset to DEFAULT_SLOTS");
        Assert.AreEqual(0f, holderRect.sizeDelta.x, 1e-3f, "holder width should reset to original");
        Assert.AreEqual(0, slotHolder.transform.childCount, "Slot child should be removed");
        Assert.IsNull(playerAnimator.runtimeAnimatorController, "Animator should be cleared after disable");
    }
}
