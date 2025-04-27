using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;

[TestFixture]

public class PorterETest 
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

    [SetUp]
    public void SetUp()
    {
        // Create Porter and dependencies
        porterObj = new GameObject("PorterObj");
        porter = porterObj.AddComponent<Porter>();

        slotPrefab = new GameObject("SlotPrefab");
        slotHolder = new GameObject("SlotHolder", typeof(RectTransform));
        grid = slotHolder.AddComponent<GridLayoutGroup>();
        holderRect = slotHolder.GetComponent<RectTransform>();
        holderRect.sizeDelta = Vector2.zero;
        grid.constraintCount = DEFAULT_SLOTS;

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
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(porterObj);
        Object.DestroyImmediate(slotPrefab);
        Object.DestroyImmediate(slotHolder);
        Object.DestroyImmediate(playerAnimator.gameObject);
    }

    [Test]
    public void OnEnable_ConfiguresInventoryAndAnimator_AndSetsEnableClass()
    {
        // Preconditions
        Assert.AreEqual(DEFAULT_SLOTS, grid.constraintCount);
        Assert.AreEqual(0f, holderRect.sizeDelta.x);
        Assert.AreEqual(0, slotHolder.transform.childCount);
        Assert.IsNull(playerAnimator.runtimeAnimatorController);
        // enableClass is private, but OnEnable should set it true
        var flagField = typeof(Porter)
            .GetField("enableClass", BindingFlags.NonPublic | BindingFlags.Instance);
        Assert.IsFalse((bool)flagField.GetValue(porter));

        // Invoke OnEnable
        typeof(Porter)
            .GetMethod("OnEnable", BindingFlags.NonPublic | BindingFlags.Instance)
            .Invoke(porter, null);

        // enableClass
        Assert.IsTrue((bool)flagField.GetValue(porter), "enableClass should be true after OnEnable");

        // Inventory: grid and rect
        Assert.AreEqual(MAX_SLOTS, grid.constraintCount, "constraintCount should be MAX_SLOTS");
        Assert.AreEqual(EXTRA_SLOT_WIDTH, holderRect.sizeDelta.x, 1e-3f,
            "holder width should increase by EXTRA_SLOT_WIDTH");

        // Slot instantiation
        Assert.AreEqual(1, slotHolder.transform.childCount, "should add one slot child");
        var newSlot = slotHolder.transform.GetChild(0).gameObject;
        Assert.AreEqual("Porter Slot", newSlot.name);
        Assert.AreEqual(Vector3.one, newSlot.transform.localScale);
        Assert.AreEqual(Vector3.zero, newSlot.transform.localPosition);
        Assert.AreEqual(Quaternion.identity, newSlot.transform.localRotation);

        // Animator assignment
        Assert.AreEqual(testController, playerAnimator.runtimeAnimatorController,
            "playerSpriteAnimator should be set to porterAnimator");
    }

    [Test]
    public void OnDisable_ResetsInventoryAndAnimator_AndClearsEnableClass()
    {
        // First enable to setup state
        typeof(Porter)
            .GetMethod("OnEnable", BindingFlags.NonPublic | BindingFlags.Instance)
            .Invoke(porter, null);
        Assert.AreEqual(1, slotHolder.transform.childCount);

        // Invoke OnDisable
        typeof(Porter)
            .GetMethod("OnDisable", BindingFlags.NonPublic | BindingFlags.Instance)
            .Invoke(porter, null);

        // enableClass
        var flagField = typeof(Porter)
            .GetField("enableClass", BindingFlags.NonPublic | BindingFlags.Instance);
        Assert.IsFalse((bool)flagField.GetValue(porter), "enableClass should be false after OnDisable");

        // Inventory reset
        Assert.AreEqual(DEFAULT_SLOTS, grid.constraintCount, "constraintCount reset to DEFAULT_SLOTS");
        Assert.AreEqual(0f, holderRect.sizeDelta.x, 1e-3f,
            "holder width reset after OnDisable");
        Assert.AreEqual(0, slotHolder.transform.childCount, "slot child should be removed");

        // Animator cleared
        Assert.IsNull(playerAnimator.runtimeAnimatorController,
            "playerSpriteAnimator.runtimeAnimatorController should be null after OnDisable");
    }
}
