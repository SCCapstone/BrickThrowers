// File: Assets/Tests/PlayMode/SharkPTest.cs
// To run this specific PlayMode test only:
//   • In the Unity Editor Test Runner:
//       – Window → General → Test Runner  
//       – Select the “PlayMode” category  
//       – Right-click “SharkPTest” → Run Selected  
//   • Via CLI (runs only SharkPTest):  
//       Unity -batchmode -projectPath . -runTests -testPlatform PlayMode \  
//         -testFilter SharkPTest -logFile -testResults TestResults/SharkPTest.xml

using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Reflection;

public class SharkPTest
{
    private GameObject sharkObj;
    private Shark shark;
    private Rigidbody2D rb;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        // Create Shark and its Rigidbody2D
        sharkObj = new GameObject("Shark");
        rb       = sharkObj.AddComponent<Rigidbody2D>();
        shark    = sharkObj.AddComponent<Shark>();

        // Inject our Rigidbody2D into the private field
        typeof(Shark)
            .GetField("rb", BindingFlags.NonPublic | BindingFlags.Instance)
            .SetValue(shark, rb);

        // Disable gravity so movement is only from Patrol()
        rb.gravityScale = 0f;

        // Wait a frame for Start() to run and Patrol() coroutine to begin
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
        // Wait one FixedUpdate to allow the Patrol coroutine to set velocity
        yield return new WaitForFixedUpdate();

        float speed = rb.velocity.magnitude;
        // Allow a wider tolerance since direction is random
        Assert.AreEqual(shark.patrolSpeed, speed, 1f,
            "After one frame, shark should be patrolling at roughly patrolSpeed");
    }
}