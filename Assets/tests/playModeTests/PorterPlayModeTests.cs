// File: Assets/Tests/playmodetest/PorterPlayModeTests.cs
// To run these PlayMode tests:
//   In the Unity Editor: Window → General → Test Runner → select “PlayMode” and click Run All
//   Via CLI:
//     Unity -batchmode -projectPath . -runTests -testPlatform PlayMode \
//       -logFile -testResults TestResults/PlayModeResults.xml

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
        obj = new GameObject("Porter");
        porter = obj.AddComponent<Porter>();
        yield return null; // wait one frame
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
        yield return null; // let any internal state settle

        Assert.IsTrue(added, "Should be able to add to empty slot");
        Assert.IsTrue(porter.HasExtraSlotItem, "HasExtraSlotItem should be true after adding");
        Assert.AreSame(testItem, porter.GetExtraSlotItem(), "GetExtraSlotItem should return the same object across frames");
    }

    [UnityTest]
    public IEnumerator RemoveExtraSlotItem_ClearsSlotAndPreventsFurtherRemoval()
    {
        var testItem = new object();
        porter.AddToExtraSlot(testItem);
        yield return null;

        var removed = porter.RemoveFromExtraSlot();
        yield return null;

        Assert.AreSame(testItem, removed, "First removal should return the item");
        Assert.IsFalse(porter.HasExtraSlotItem, "Slot should be empty after removal");

        var removedAgain = porter.RemoveFromExtraSlot();
        Assert.IsNull(removedAgain, "Removing again should return null");
    }
}
