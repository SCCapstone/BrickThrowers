// File: Assets/Tests/editmodetest/OctopusUnitTests.cs
// To run these EditMode tests:
//   In the Unity Editor: Window → General → Test Runner → select “EditMode” and click Run All
//   Via CLI:
//     Unity -batchmode -projectPath . -runTests -testPlatform EditMode -logFile -testResults TestResults/EditModeResults.xml

using NUnit.Framework;
using UnityEngine;

[TestFixture]
public class OctopusUnitTests
{
    GameObject octoObj;
    Octopus octo;

    [SetUp]
    public void SetUp()
    {
        octoObj = new GameObject();
        octo = octoObj.AddComponent<Octopus>();
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(octoObj);
    }

    [Test]
    public void DefaultValues_AreCorrect()
    {
        Assert.AreEqual(3f, octo.detectionRange);
        Assert.AreEqual(5f, octo.latchDuration);
        Assert.AreEqual(60, octo.health);
        Assert.AreEqual(2f, octo.moveSpeed);
    }

    [Test]
    public void TakeDamage_ReducesHealth()
    {
        int before = octo.health;
        octo.TakeDamage(15);
        Assert.AreEqual(before - 15, octo.health);
    }

    [Test]
    public void TakeDamage_AllowsNegativeHealth()
    {
        octo.health = 5;
        octo.TakeDamage(10);
        Assert.Less(octo.health, 0);
    }
}
