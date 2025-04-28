// File: Assets/Tests/PlayMode/LionFishPTest.cs

using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Reflection;

public class LionFishPTest 
{
    private GameObject fishObj;
    private Lionfish fish;
    private Rigidbody2D fishRb;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        fishObj = new GameObject("Lionfish");
        fishRb  = fishObj.AddComponent<Rigidbody2D>();
        fishObj.AddComponent<BoxCollider2D>();
        fish     = fishObj.AddComponent<Lionfish>();

        yield return null;
    }

    [UnityTearDown]
    public IEnumerator TearDown()
    {
        Object.Destroy(fishObj);
        yield return null;
    }

    [UnityTest]
    public IEnumerator Patrol_SetsVelocityMagnitudeToPatrolSpeed()
    {
        yield return null;
        Assert.AreEqual(fish.patrolSpeed, fishRb.velocity.magnitude, 0.1f);
    }

    [UnityTest]
    public IEnumerator ChangesDirection_AfterInterval()
    {
        var dirField = typeof(Lionfish)
            .GetField("patrolDirection", BindingFlags.NonPublic | BindingFlags.Instance);
        Vector2 initial = (Vector2)dirField.GetValue(fish);

        yield return new WaitForSeconds(fish.directionChangeInterval + 0.1f);

        Vector2 updated = (Vector2)dirField.GetValue(fish);
        Assert.AreNotEqual(initial, updated);
    }
}
