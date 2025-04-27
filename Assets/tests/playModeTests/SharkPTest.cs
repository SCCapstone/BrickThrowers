using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class SharkPTest
{
    private GameObject sharkObj;
    private Shark shark;
    private Rigidbody2D rb;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        // Create the Shark and give it a Rigidbody2D
        sharkObj = new GameObject("Shark");
        rb       = sharkObj.AddComponent<Rigidbody2D>();
        shark    = sharkObj.AddComponent<Shark>();

        // Manually inject our rb into the private field so Start() and Patrol() use it
        typeof(Shark)
            .GetField("rb", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(shark, rb);

        // Let Start() run (initializes patrolDirection) and first Update() schedule Patrol()
        yield return null;
    }

    [UnityTearDown]
    public IEnumerator TearDown()
    {
        Object.Destroy(sharkObj);
        yield return null;
    }

    [UnityTest]
    public IEnumerator Shark_Patrol_SetsVelocityMagnitude()
    {
        // After one more frame, the Patrol coroutine should have run and set velocity
        yield return null;

        // The magnitude of rb.velocity should equal the patrolSpeed
        Assert.AreEqual(shark.patrolSpeed, rb.velocity.magnitude, 0.1f,
            "After one frame, shark should be patrolling at patrolSpeed");
    }
}
