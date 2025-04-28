// File: Assets/Tests/PlayMode/PlayerTests.cs
// To run this specific PlayMode test only:
//   • In the Unity Editor Test Runner:
//       – Window → General → Test Runner  
//       – Select the “PlayMode” category  
//       – Right-click “PlayerTests” → Run Selected  
//   • Via CLI (runs only PlayerTests):  
//       Unity -batchmode -projectPath . -runTests -testPlatform PlayMode \
//         -testFilter PlayerTests -logFile -testResults TestResults/PlayerTests.xml

using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PlayerTests
{
    private GameObject playerObject;
    private Player player;

    [UnitySetUp]
    public IEnumerator Setup()
    {
        // Create a plain GameObject and attach your Player script
        playerObject = new GameObject("PlayerTest");
        player = playerObject.AddComponent<Player>();

        // Ensure there's a Camera.main
        if (Camera.main == null)
        {
            var cam = new GameObject("Main Camera");
            cam.tag = "MainCamera";
            cam.transform.position = new Vector3(0f, 0f, -50f);
            cam.AddComponent<Camera>();
        }

        // Let the scene initialize (Awake/Start)
        yield return null;
    }

    [UnityTearDown]
    public IEnumerator Teardown()
    {
        Object.Destroy(playerObject);
        yield return null;
    }

    [UnityTest]
    public IEnumerator PlayerTest()
    {
        // A trivial smoke‐test that your Player component exists and Awake/Start didn’t blow up
        Assert.IsNotNull(player, "Player component should have been added in Setup");
        yield return null;
    }
}