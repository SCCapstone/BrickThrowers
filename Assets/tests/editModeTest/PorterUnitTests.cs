// File: Assets/Tests/editmodetest/PorterUnitTests.cs
// To run these EditMode tests:
//   In the Unity Editor: Window → General → Test Runner → select “EditMode” and click Run All
//   Via CLI:
//     Unity -batchmode -projectPath . -runTests -testPlatform EditMode \
//       -logFile -testResults TestResults/EditModeResults.xml

using NUnit.Framework;
using UnityEngine;

[TestFixture]
public class PorterUnitTests
{
    private GameObject obj;
    private Porter porter;

    [SetUp]
    public void SetUp()
    {
        obj = new GameObject("Porter");
        porter = obj.AddComponent<Porter>();
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(obj);
    }

    [Test]
    public void HasExtraSlotItem_IsFalseInitially()
    {
        Assert.IsFalse(porter.HasExtraSlotItem, "Porter should start with an empty extra slot");
    }

    [Test]
    public void AddToExtraSlot_WhenEmpty_ReturnsTrueAndStoresItem()
    {
        var item = new object();
        bool result = porter.AddToExtraSlot(item);

        Assert.IsTrue(result, "AddToExtraSlot should return true when slot is empty");
        Assert.IsTrue(porter.HasExtraSlotItem, "HasExtraSlotItem should be true after adding");
        Assert.AreSame(item, porter.GetExtraSlotItem(), "GetExtraSlotItem should return the added object");
    }

    [Test]
    public void AddToExtraSlot_WhenOccupied_ReturnsFalseAndDoesNotOverwrite()
    {
        var first = new object();
        var second = new object();
        porter.AddToExtraSlot(first);
        bool result = porter.AddToExtraSlot(second);

        Assert.IsFalse(result, "AddToExtraSlot should return false when slot is occupied");
        Assert.AreSame(first, porter.GetExtraSlotItem(), "Existing item should remain unchanged");
    }

    [Test]
    public void RemoveFromExtraSlot_WhenHasItem_ReturnsItemAndClearsSlot()
    {
        var item = new object();
        porter.AddToExtraSlot(item);
        object removed = porter.RemoveFromExtraSlot();

        Assert.AreSame(item, removed, "RemoveFromExtraSlot should return the previously added item");
        Assert.IsFalse(porter.HasExtraSlotItem, "Slot should be empty after removal");
    }

    [Test]
    public void RemoveFromExtraSlot_WhenEmpty_ReturnsNull()
    {
        object removed = porter.RemoveFromExtraSlot();
        Assert.IsNull(removed, "RemoveFromExtraSlot should return null when slot is already empty");
    }
}
