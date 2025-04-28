// File: Assets/Tests/PlayMode/HarpoonerPTest.cs
// To run this specific PlayMode test only:
//   • In the Unity Editor Test Runner:
//       – Window → General → Test Runner  
//       – Select the “PlayMode” category  
//       – Right-click “HarpoonerPTest” → Run Selected  
//   • Via CLI (runs only HarpoonerPTest):  
//       Unity -batchmode -projectPath . -runTests -testPlatform PlayMode \  
//         -testFilter HarpoonerPTest -logFile -testResults TestResults/HarpoonerPTest.xml

using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Reflection;

public class HarpoonerPTest
{
    private GameObject go;
    private Harpooner harpooner;
    private GameObject attackZone;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        // Create the GameObject and keep it inactive for injection
        go = new GameObject("Harpooner");
        go.SetActive(false);
        go.AddComponent<Animator>(); // placeholder for private `animator`
        harpooner = go.AddComponent<Harpooner>();

        // Inject the attackZone
        attackZone = new GameObject("AttackZone");
        attackZone.SetActive(false);
        typeof(Harpooner)
            .GetField("attackZone", BindingFlags.NonPublic | BindingFlags.Instance)
            .SetValue(harpooner, attackZone);

        // Avoid the AnimatorOverrideController error failing the test
        LogAssert.Expect(LogType.Error, "Could not set Runtime Animator Controller");

        // Activate so Awake() and OnEnable() run
        go.SetActive(true);
        yield return null;
    }

    [UnityTearDown]
    public IEnumerator TearDown()
    {
        Object.Destroy(go);
        Object.Destroy(attackZone);
        yield return null;
    }

    [UnityTest]
    public IEnumerator OnEnable_ActivatesZone()
    {
        // Only check the zone activation
        Assert.IsTrue(attackZone.activeSelf, "attackZone should be active after OnEnable");
        yield break;
    }

    [UnityTest]
    public IEnumerator OnDisable_DeactivatesZone()
    {
        // Disable component to trigger OnDisable
        harpooner.enabled = false;
        yield return null;

        Assert.IsFalse(attackZone.activeSelf, "attackZone should be inactive after OnDisable");
        yield break;
    }
}
