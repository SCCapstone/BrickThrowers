// File: Assets/Tests/Editor/PorterETest.cs
// To run this specific EditMode test only:
//   • In the Unity Editor Test Runner:
//       – Window → General → Test Runner  
//       – Select “EditMode” category  
//       – Right-click “PorterETest” → Run Selected  
//   • Via CLI (runs only PorterETest):  
//       Unity -batchmode -projectPath . -runTests -testPlatform EditMode \  
//         -testFilter PorterETest -logFile -testResults TestResults/PorterETest.xml

using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.TestTools;
using System.Reflection;
using System.Collections;

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
        porterObj = new GameObject("PorterObj");
        porter    = porterObj.AddComponent<Porter>();

        slotPrefab = new GameObject("SlotPrefab");
        slotHolder = new GameObject("SlotHolder", typeof(RectTransform));
        grid       = slotHolder.AddComponent<GridLayoutGroup>();
        holderRect = slotHolder.GetComponent<RectTransform>();
        grid.constraintCount = DEFAULT_SLOTS;
        holderRect.sizeDelta = Vector2.zero;

        var playerGO      = new GameObject("PlayerSprite");
        playerAnimator    = playerGO.AddComponent<Animator>();
        testController    = new AnimatorOverrideController();

        var t = typeof(Porter);
        t.GetField("slot",               BindingFlags.NonPublic | BindingFlags.Instance).SetValue(porter, slotPrefab);
        t.GetField("slotHolder",         BindingFlags.NonPublic | BindingFlags.Instance).SetValue(porter, slotHolder);
        t.GetField("porterAnimator",     BindingFlags.NonPublic | BindingFlags.Instance).SetValue(porter, testController);
        t.GetField("playerSpriteAnimator", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(porter, playerAnimator);
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
        // Swallow Unity error about invalid AnimatorOverrideController
        LogAssert.Expect(LogType.Error, "Could not set Runtime Animator Controller");

        // Preconditions
        Assert.AreEqual(DEFAULT_SLOTS, grid.constraintCount);
        Assert.AreEqual(0f,            holderRect.sizeDelta.x);
        Assert.AreEqual(0,             slotHolder.transform.childCount);
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
        Assert.AreEqual(EXTRA_SLOT_WIDTH, holderRect.sizeDelta.x, 1e-3f,
            "holder width should increase by EXTRA_SLOT_WIDTH");
        Assert.AreEqual(1, slotHolder.transform.childCount, "should add one slot child");
    }

    [UnityTest]
    public IEnumerator OnDisable_ResetsInventoryAndAnimator_AndClearsEnableClass()
    {
        // First enable (swallow the same Animator error)
        LogAssert.Expect(LogType.Error, "Could not set Runtime Animator Controller");
        typeof(Porter)
            .GetMethod("OnEnable", BindingFlags.NonPublic | BindingFlags.Instance)
            .Invoke(porter, null);
        yield return null;

        // Act
        typeof(Porter)
            .GetMethod("OnDisable", BindingFlags.NonPublic | BindingFlags.Instance)
            .Invoke(porter, null);
        yield return null;

        // Assertions
        var flagField = typeof(Porter)
            .GetField("enableClass", BindingFlags.NonPublic | BindingFlags.Instance);
        Assert.IsFalse((bool)flagField.GetValue(porter), "enableClass should be false after OnDisable");
        Assert.AreEqual(DEFAULT_SLOTS, grid.constraintCount, "constraintCount reset to DEFAULT_SLOTS");
        Assert.AreEqual(0f,            holderRect.sizeDelta.x, 1e-3f, "holder width reset after OnDisable");
        Assert.AreEqual(0,             slotHolder.transform.childCount, "slot child should be removed");
    }
}
