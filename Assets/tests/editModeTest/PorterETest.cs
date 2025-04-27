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
        slotHolder  = new GameObject("SlotHolder", typeof(RectTransform));
        grid        = slotHolder.AddComponent<GridLayoutGroup>();
        holderRect  = slotHolder.GetComponent<RectTransform>();

        // Initialize holder layout
        holderRect.sizeDelta    = Vector2.zero;
        grid.constraintCount    = DEFAULT_SLOTS;

        // Dummy player animator + test controller
        var playerObj     = new GameObject("Player");
        playerAnimator    = playerObj.AddComponent<Animator>();
        testController    = new AnimatorOverrideController();

        // Inject all serialized fields
        var t = typeof(Porter);
        t.GetField("slot",                 BindingFlags.NonPublic|BindingFlags.Instance)
            .SetValue(porter, slotPrefab);
        t.GetField("slotHolder",           BindingFlags.NonPublic|BindingFlags.Instance)
            .SetValue(porter, slotHolder);
        t.GetField("porterAnimator",       BindingFlags.NonPublic|BindingFlags.Instance)
            .SetValue(porter, testController);
        t.GetField("playerSpriteAnimator", BindingFlags.NonPublic|BindingFlags.Instance)
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
        Assert.AreEqual(0f,           holderRect.sizeDelta.x);
        Assert.AreEqual(0,            slotHolder.transform.childCount);
        Assert.IsNull(playerAnimator.runtimeAnimatorController);

        // Check enableClass defaults to false
        var flagField = typeof(Porter)
            .GetField("enableClass", BindingFlags.NonPublic|BindingFlags.Instance);
        Assert.IsFalse((bool)flagField.GetValue(porter));

        // Act → OnEnable
        typeof(Porter)
            .GetMethod("OnEnable", BindingFlags.NonPublic|BindingFlags.Instance)
            .Invoke(porter, null);

        // enableClass should now be true
        Assert.IsTrue((bool)flagField.GetValue(porter));

        // Inventory grid & width
        Assert.AreEqual(MAX_SLOTS,        grid.constraintCount);
        Assert.AreEqual(EXTRA_SLOT_WIDTH, holderRect.sizeDelta.x, 1e-3f);

        // Exactly one slot child
        Assert.AreEqual(1, slotHolder.transform.childCount);
        var newSlot = slotHolder.transform.GetChild(0).gameObject;
        Assert.AreEqual("Porter Slot",       newSlot.name);
        Assert.AreEqual(Vector3.one,         newSlot.transform.localScale);
        Assert.AreEqual(Vector3.zero,        newSlot.transform.localPosition);
        Assert.AreEqual(Quaternion.identity, newSlot.transform.localRotation);

        // Animator assignment
        Assert.AreEqual(testController, playerAnimator.runtimeAnimatorController);
    }

    [UnityTest]
    public IEnumerator OnDisable_ResetsInventoryAndAnimator_AndClearsEnableClass()
    {
        // First bring it up
        typeof(Porter)
            .GetMethod("OnEnable", BindingFlags.NonPublic|BindingFlags.Instance)
            .Invoke(porter, null);
        yield return null; // let SetPorterInventory finish

        // Act → OnDisable
        typeof(Porter)
            .GetMethod("OnDisable", BindingFlags.NonPublic|BindingFlags.Instance)
            .Invoke(porter, null);
        yield return null; // allow Destroy() to complete

        // enableClass false again
        var flagField = typeof(Porter)
            .GetField("enableClass", BindingFlags.NonPublic|BindingFlags.Instance);
        Assert.IsFalse((bool)flagField.GetValue(porter));

        // Inventory reset
        Assert.AreEqual(DEFAULT_SLOTS, grid.constraintCount);
        Assert.AreEqual(0f,            holderRect.sizeDelta.x, 1e-3f);
        Assert.AreEqual(0,             slotHolder.transform.childCount);

        // Animator cleared
        Assert.IsNull(playerAnimator.runtimeAnimatorController);

        yield break;
    }
}
