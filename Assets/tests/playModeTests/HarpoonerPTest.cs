// File: Assets/Tests/PlayMode/HarpoonerPTest.cs
// To run this specific PlayMode test only:
//   • In the Unity Editor Test Runner:
//       – Window → General → Test Runner  
//       – Select “PlayMode” category  
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
    private Animator playerAnimator;
    private RuntimeAnimatorController testController;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        // Create GameObject and keep inactive for injection
        go = new GameObject("Harpooner");
        go.AddComponent<Animator>(); // placeholder for private animator
        harpooner = go.AddComponent<Harpooner>();
        go.SetActive(false);

        // Inject attackZone
        attackZone = new GameObject("AttackZone");
        typeof(Harpooner)
            .GetField("attackZone", BindingFlags.NonPublic | BindingFlags.Instance)
            .SetValue(harpooner, attackZone);

        // Inject playerSpriteAnimator & harpoonerAnimator
        var playerGO = new GameObject("PlayerSprite");
        playerAnimator = playerGO.AddComponent<Animator>();
        testController = new AnimatorOverrideController();
        typeof(Harpooner)
            .GetField("harpoonerAnimator", BindingFlags.NonPublic | BindingFlags.Instance)
            .SetValue(harpooner, testController);
        typeof(Harpooner)
            .GetField("playerSpriteAnimator", BindingFlags.NonPublic | BindingFlags.Instance)
            .SetValue(harpooner, playerAnimator);

        // Activate to trigger Awake and OnEnable, avoiding NRE
        go.SetActive(true);
        typeof(Harpooner)
            .GetMethod("Awake", BindingFlags.NonPublic | BindingFlags.Instance)
            .Invoke(harpooner, null);

        yield return null;
    }

    [UnityTearDown]
    public IEnumerator TearDown()
    {
        Object.Destroy(go);
        Object.Destroy(attackZone);
        Object.Destroy(playerAnimator.gameObject);
        yield return null;
    }

    [UnityTest]
    public IEnumerator OnEnable_ActivatesZone()
    {
        // OnEnable ran in SetUp
        Assert.IsTrue(attackZone.activeSelf, "attackZone should be active after OnEnable");
        yield break;
    }

    [UnityTest]
    public IEnumerator OnDisable_DeactivatesZone()
    {
        // Disable to trigger OnDisable
        harpooner.enabled = false;
        yield return null;
        Assert.IsFalse(attackZone.activeSelf, "attackZone should be inactive after OnDisable");
        yield break;
    }
}
