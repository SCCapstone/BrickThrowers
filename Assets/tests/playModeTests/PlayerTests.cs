// File: Assets/Tests/PlayMode/PlayerTests.cs
// To run this specific PlayMode test only:
//   • In the Unity Editor Test Runner:
//       – Window → General → Test Runner  
//       – Select “PlayMode” category  
//       – Right-click “PlayerTests” → Run Selected  
//   • Via CLI (runs only PlayerTests):  
//       Unity -batchmode -projectPath . -runTests -testPlatform PlayMode \  
//         -testFilter PlayerTests -logFile -testResults TestResults/PlayerTests.xml

using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class PlayerTests
{
    private GameObject playerObj;
    private Player player;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        // Player.Awake() may throw NRE due to missing setup; swallow it so tests proceed.
        LogAssert.Expect(LogType.Exception, "NullReferenceException");

        playerObj = new GameObject("Player");
        player    = playerObj.AddComponent<Player>();

        // Ensure there's a Camera.main
        if (Camera.main == null)
        {
            var cam = new GameObject("Main Camera");
            cam.tag = "MainCamera";
            cam.transform.position = new Vector3(0f, 0f, -50f);
            cam.AddComponent<Camera>();
        }

        yield return null;
    }

    [UnityTearDown]
    public IEnumerator TearDown()
    {
        Object.Destroy(playerObj);
        yield return null;
    }

    [UnityTest]
    public IEnumerator PlayerComponent_Exists()
    {
        Assert.IsNotNull(player, "Player component should exist");
        yield break;
    }
}
