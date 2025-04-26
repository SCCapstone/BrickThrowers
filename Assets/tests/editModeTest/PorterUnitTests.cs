// File: Assets/Tests/editmodetest/PorterUnitTests.cs
// To run this specific EditMode test only:
//   • In the Unity Editor Test Runner:
//       – Window → General → Test Runner  
//       – Select “EditMode” category  
//       – Right-click “PorterUnitTests” → Run Selected  
//   • Via CLI (runs only PorterUnitTests):
//       Unity -batchmode -projectPath . -runTests -testPlatform EditMode \
//         -testFilter PorterUnitTests -logFile -testResults TestResults/PorterUnitTests.xml

using NUnit.Framework;
using UnityEngine;
using System.Reflection;

[TestFixture]
public class PorterUnitTests
{
    private GameObject obj;
    private Porter porter;

    [SetUp]
    public void SetUp()
    {
        obj = new GameObject("PorterObj");
        porter = obj.AddComponent<Porter>();
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(obj);
    }

    [Test]
    public void OnEnable_SetsEnableClassTrue()
    {
        // initially the component has just been added and OnEnable has run
        var field = typeof(Porter).GetField("enableClass", BindingFlags.NonPublic | BindingFlags.Instance);
        bool enabledState = (bool)field.GetValue(porter);
        Assert.IsTrue(enabledState, "enableClass should be true after OnEnable");
    }

    [Test]
    public void OnDisable_SetsEnableClassFalse()
    {
        // invoke OnDisable via reflection
        MethodInfo onDisable = typeof(Porter).GetMethod("OnDisable", BindingFlags.NonPublic | BindingFlags.Instance);
        onDisable.Invoke(porter, null);

        var field = typeof(Porter).GetField("enableClass", BindingFlags.NonPublic | BindingFlags.Instance);
        bool enabledState = (bool)field.GetValue(porter);
        Assert.IsFalse(enabledState, "enableClass should be false after OnDisable");
    }

    [Test]
    public void AddToExtraSlot_WhenEmpty_ReturnsTrueAndStoresItem()
    {
        var item = new object();
        bool result = porter.AddToExtraSlot(item);
        Assert.IsTrue(result, "Should return true when slot is empty");
        Assert.IsTrue(porter.HasExtraSlotItem, "HasExtraSlotItem should be true after adding");
        Assert.AreSame(item, porter.GetExtraSlotItem(), "GetExtraSlotItem must return the added item");
    }

    [Test]
    public void AddToExtraSlot_WhenOccupied_ReturnsFalse()
    {
        var first = new object();
        var second = new object();
        porter.AddToExtraSlot(first);
        bool result = porter.AddToExtraSlot(second);
        Assert.IsFalse(result, "Should return false when slot already has an item");
        Assert.AreSame(first, porter.GetExtraSlotItem(), "Original item should remain unchanged");
    }

    [Test]
    public void RemoveFromExtraSlot_WhenHasItem_ReturnsItemAndClearsSlot()
    {
        var item = new object();
        porter.AddToExtraSlot(item);
        object removed = porter.RemoveFromExtraSlot();
        Assert.AreSame(item, removed, "Should return the removed item");
        Assert.IsFalse(porter.HasExtraSlotItem, "Slot should be empty after removal");
    }

    [Test]
    public void RemoveFromExtraSlot_WhenEmpty_ReturnsNull()
    {
        object removed = porter.RemoveFromExtraSlot();
        Assert.IsNull(removed, "Should return null when slot is already empty");
    }
}
