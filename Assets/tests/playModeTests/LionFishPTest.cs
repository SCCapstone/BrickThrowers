// File: Assets/Tests/PlayMode/LionFishPTest.cs
// To run this specific PlayMode test only:
//   • In the Unity Editor Test Runner:  
//       – Window → General → Test Runner  
//       – Select the “PlayMode” category  
//       – Right-click “LionFishPTest” → Run Selected  
//   • Via CLI (runs only LionFishPTest):  
//       Unity -batchmode -projectPath . -runTests -testPlatform PlayMode \
//         -testFilter LionFishPTest -logFile -testResults TestResults/LionFishPTest.xml

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

        // Wait a frame so Start() and first Update() run
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
        // After one frame, Patrol() should have set velocity
        yield return null;

        Assert.AreEqual(fish.patrolSpeed, fishRb.velocity.magnitude, 0.1f,
            "After one frame, lionfish should patrol at patrolSpeed");
    }

    [UnityTest]
    public IEnumerator ChangesDirection_AfterInterval()
    {
        // Capture initial private patrolDirection
        var dirField = typeof(Lionfish)
            .GetField("patrolDirection", BindingFlags.NonPublic | BindingFlags.Instance);
        Vector2 initial = (Vector2)dirField.GetValue(fish);

        // Wait longer than directionChangeInterval
        yield return new WaitForSeconds(fish.directionChangeInterval + 0.1f);

        Vector2 updated = (Vector2)dirField.GetValue(fish);
        Assert.AreNotEqual(initial, updated,
            "patrolDirection should change after directionChangeInterval");
    }
}
