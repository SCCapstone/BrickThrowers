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

    const int MAX_SLOTS = 4;
    const int DEFAULT_SLOTS = 3;
    const float EXTRA_SLOT_WIDTH = 95f;

    [SetUp]
    public void SetUp()
    {
        // Create the Porter and slot objects
        porterObj  = new GameObject("PorterObj");
        porter     = porterObj.AddComponent<Porter>();

        slotPrefab = new GameObject("SlotPrefab");
        slotHolder = new GameObject("SlotHolder", typeof(RectTransform));
        grid       = slotHolder.AddComponent<GridLayoutGroup>();
        holderRect = slotHolder.GetComponent<RectTransform>();

        // Initialize defaults
        grid.constraintCount = DEFAULT_SLOTS;
        holderRect.sizeDelta = Vector2.zero;

        // Inject slot references
        var t = typeof(Porter);
        t.GetField("slot",        BindingFlags.NonPublic | BindingFlags.Instance)
         .SetValue(porter, slotPrefab);
        t.GetField("slotHolder",  BindingFlags.NonPublic | BindingFlags.Instance)
         .SetValue(porter, slotHolder);

        // Null out animator fields to prevent assignment errors
        t.GetField("porterAnimator",       BindingFlags.NonPublic | BindingFlags.Instance)
         .SetValue(porter, null);
        t.GetField("playerSpriteAnimator", BindingFlags.NonPublic | BindingFlags.Instance)
         .SetValue(porter, null);
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(porterObj);
        Object.DestroyImmediate(slotPrefab);
        Object.DestroyImmediate(slotHolder);
    }

    [Test]
    public void OnEnable_ConfiguresInventoryAndSetsEnableClass()
    {
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
        Assert.IsTrue((bool)flagField.GetValue(porter), "enableClass should be true");
        Assert.AreEqual(MAX_SLOTS,        grid.constraintCount,  "constraintCount should be MAX_SLOTS");
        Assert.AreEqual(EXTRA_SLOT_WIDTH, holderRect.sizeDelta.x, 1e-3f, "holder width should increase");
        Assert.AreEqual(1,                 slotHolder.transform.childCount, "one slot should be added");
    }

    [Test]
    public void OnDisable_ResetsInventoryAndClearsEnableClass()
    {
        // First enable to set state
        typeof(Porter)
            .GetMethod("OnEnable", BindingFlags.NonPublic | BindingFlags.Instance)
            .Invoke(porter, null);

        // Act
        typeof(Porter)
            .GetMethod("OnDisable", BindingFlags.NonPublic | BindingFlags.Instance)
            .Invoke(porter, null);

        // Assertions
        var flagField = typeof(Porter)
            .GetField("enableClass", BindingFlags.NonPublic | BindingFlags.Instance);
        Assert.IsFalse((bool)flagField.GetValue(porter), "enableClass should be false after OnDisable");
        Assert.AreEqual(DEFAULT_SLOTS, grid.constraintCount, "constraintCount should reset to DEFAULT_SLOTS");
        Assert.AreEqual(0f,            holderRect.sizeDelta.x, 1e-3f, "holder width should reset");
        Assert.AreEqual(0,             slotHolder.transform.childCount, "slot should be removed");
    }
}
