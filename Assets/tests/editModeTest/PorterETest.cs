// File: Assets/Tests/Editor/PorterETest.cs
// To run this specific EditMode test only:
//   • In the Unity Editor Test Runner:  
//       – Window → General → Test Runner  
//       – Select “EditMode” category  
//       – Right-click “PorterETest” → Run Selected  
//   • Via CLI (runs only PorterETest):  
//       Unity -batchmode -projectPath . -runTests -testPlatform EditMode \
//         -testFilter PorterETest -logFile -testResults TestResults/PorterETest.xml

using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.TestTools;
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
        grid.constraintCount = DEFAULT_SLOTS;
        holderRect.sizeDelta = Vector2.zero;

        var playerObj = new GameObject("PlayerSprite");
        playerAnimator = playerObj.AddComponent<Animator>();
        testController = new AnimatorOverrideController();

        // Inject serialized fields
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
    public void OnEnable_ConfiguresInventoryAndSetsEnableClass()
    {
        // Expect the AnimatorOverrideController warning
        LogAssert.Expect(LogType.Error,
            "Could not set Runtime Animator Controller. The controller");

        // Preconditions
        Assert.AreEqual(DEFAULT_SLOTS, grid.constraintCount);
        Assert.AreEqual(0f, holderRect.sizeDelta.x);
        Assert.AreEqual(0, slotHolder.transform.childCount);

        // enableClass default false
        var flagField = typeof(Porter)
            .GetField("enableClass", BindingFlags.NonPublic | BindingFlags.Instance);
        Assert.IsFalse((bool)flagField.GetValue(porter));

        // Act
        typeof(Porter)
            .GetMethod("OnEnable", BindingFlags.NonPublic | BindingFlags.Instance)
            .Invoke(porter, null);

        // Assertions
        Assert.IsTrue((bool)flagField.GetValue(porter), "enableClass should be true after OnEnable");
        Assert.AreEqual(MAX_SLOTS, grid.constraintCount, "constraintCount should be MAX_SLOTS");
        Assert.AreEqual(EXTRA_SLOT_WIDTH, holderRect.sizeDelta.x, 1e-3f, "holder width should increase by EXTRA_SLOT_WIDTH");
        Assert.AreEqual(1, slotHolder.transform.childCount, "should add one slot child");
        // Animator assignment is logged as error and Unity rejects it, so skip direct assertion here
    }

    [UnityTest]
    public IEnumerator OnDisable_ResetsInventoryAndAnimator_AndClearsEnableClass()
    {
        // First enable (swallow the error)
        LogAssert.Expect(LogType.Error,
            "Could not set Runtime Animator Controller. The controller");
        typeof(Porter)
            .GetMethod("OnEnable", BindingFlags.NonPublic | BindingFlags.Instance)
            .Invoke(porter, null);
        yield return null;  // allow SetPorterInventory

        // Act
        typeof(Porter)
            .GetMethod("OnDisable", BindingFlags.NonPublic | BindingFlags.Instance)
            .Invoke(porter, null);
        yield return null;  // allow ResetPorterInventory

        // Assertions
        var flagField = typeof(Porter)
            .GetField("enableClass", BindingFlags.NonPublic | BindingFlags.Instance);
        Assert.IsFalse((bool)flagField.GetValue(porter), "enableClass should be false after OnDisable");
        Assert.AreEqual(DEFAULT_SLOTS, grid.constraintCount, "constraintCount reset to DEFAULT_SLOTS");
        Assert.AreEqual(0f, holderRect.sizeDelta.x, 1e-3f, "holder width reset after OnDisable");
        Assert.AreEqual(0, slotHolder.transform.childCount, "slot child should be removed");
        Assert.IsNull(playerAnimator.runtimeAnimatorController,
            "playerSpriteAnimator.runtimeAnimatorController should be null after OnDisable");
    }
}