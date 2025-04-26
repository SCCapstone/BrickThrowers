// File: Assets/Tests/playmodetest/PorterPlayModeTests.cs
// To run this specific PlayMode test only:
//   • In the Unity Editor Test Runner:
//       – Window → General → Test Runner  
//       – Select “PlayMode” category  
//       – Right-click “PorterPlayModeTests” → Run Selected  
//   • Via CLI (runs only PorterPlayModeTests):
//       Unity -batchmode -projectPath . -runTests -testPlatform PlayMode \
//         -testFilter PorterPlayModeTests -logFile -testResults TestResults/PorterPlayModeTests.xml

using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class PorterPlayModeTests
{
    private GameObject obj;
    private Porter porter;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        obj = new GameObject("PorterObj");
        porter = obj.AddComponent<Porter>();
        yield return null; // wait one frame for OnEnable
    }

    [UnityTearDown]
    public IEnumerator TearDown()
    {
        Object.Destroy(obj);
        yield return null;
    }

    [UnityTest]
    public IEnumerator AddAndGetExtraSlotItem_PersistsAcrossFrames()
    {
        var testItem = new object();
        bool added = porter.AddToExtraSlot(testItem);
        yield return null; // let any state settle

        Assert.IsTrue(added, "Should successfully add to empty slot");
        Assert.IsTrue(porter.HasExtraSlotItem, "HasExtraSlotItem should be true after adding");
        Assert.AreSame(testItem, porter.GetExtraSlotItem(), "GetExtraSlotItem returns the same object across frames");
    }

    [UnityTest]
    public IEnumerator RemoveExtraSlotItem_ClearsSlotAndPreventsFurtherRemoval()
    {
        var testItem = new object();
        porter.AddToExtraSlot(testItem);
        yield return null;

        var removed = porter.RemoveFromExtraSlot();
        yield return null;

        Assert.AreSame(testItem, removed, "First removal returns the item");
        Assert.IsFalse(porter.HasExtraSlotItem, "Slot should be empty after removal");

        var removedAgain = porter.RemoveFromExtraSlot();
        Assert.IsNull(removedAgain, "Second removal should return null");
    }
}
