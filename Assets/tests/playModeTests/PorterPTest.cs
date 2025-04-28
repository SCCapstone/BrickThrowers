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
    private GameObject slotPrefab;
    private GameObject slotHolder;
    private GridLayoutGroup grid;
    private RectTransform holderRect;

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

        // Initialize holder to defaults
        grid.constraintCount = 3;             // DEFAULT_SLOTS
        holderRect.sizeDelta = Vector2.zero;

        // Inject private fields via reflection
        var t = typeof(Porter);
        t.GetField("slot",        BindingFlags.NonPublic|BindingFlags.Instance)
         .SetValue(porter, slotPrefab);
        t.GetField("slotHolder",  BindingFlags.NonPublic|BindingFlags.Instance)
         .SetValue(porter, slotHolder);

        yield break; // we won't rely on OnEnable/OnDisable
    }

    [UnityTearDown]
    public IEnumerator TearDown()
    {
        Object.Destroy(porterObj);
        Object.Destroy(slotPrefab);
        Object.Destroy(slotHolder);
        yield return null;
    }

    [UnityTest]
    public IEnumerator OnEnable_ConfiguresInventory()
    {
        // Manually invoke the inventory setup method
        typeof(Porter)
            .GetMethod("SetPorterInventory", BindingFlags.NonPublic|BindingFlags.Instance)
            .Invoke(porter, null);
        yield return null;

        Assert.AreEqual(4,       grid.constraintCount,      "constraintCount should be MAX_SLOTS (4)");
        Assert.AreEqual(95f,     holderRect.sizeDelta.x, 1e-3f, "holder width should increase by EXTRA_SLOT_WIDTH (95)");
        Assert.AreEqual(1,       slotHolder.transform.childCount, "One slot should be added");
    }

    [UnityTest]
    public IEnumerator OnDisable_ResetsInventory()
    {
        // First set up inventory
        typeof(Porter)
            .GetMethod("SetPorterInventory", BindingFlags.NonPublic|BindingFlags.Instance)
            .Invoke(porter, null);
        yield return null;

        // Then manually invoke the reset method
        typeof(Porter)
            .GetMethod("ResetPorterInventory", BindingFlags.NonPublic|BindingFlags.Instance)
            .Invoke(porter, null);
        yield return null;

        Assert.AreEqual(3,      grid.constraintCount,      "constraintCount should reset to DEFAULT_SLOTS (3)");
        Assert.AreEqual(0f,     holderRect.sizeDelta.x, 1e-3f, "holder width should reset to 0");
        Assert.AreEqual(0,      slotHolder.transform.childCount, "Slot child should be removed");
    }
}
