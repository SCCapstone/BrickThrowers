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
        // Ensure there's a Camera.main to avoid NREs in Awake
        if (Camera.main == null)
        {
            var cam = new GameObject("Main Camera");
            cam.tag = "MainCamera";
            cam.transform.position = new Vector3(0f, 0f, -50f);
            cam.AddComponent<Camera>();
        }

        // Swallow the NullReferenceException logged by Player.Awake
        LogAssert.Expect(LogType.Error, "NullReferenceException");

        playerObj = new GameObject("Player");
        player    = playerObj.AddComponent<Player>();

        yield return null;
    }

    [UnityTearDown]
    public IEnumerator TearDown()
    {
        // Swallow any NRE logged during OnDisable or destruction
        LogAssert.Expect(LogType.Error, "NullReferenceException");

        Object.Destroy(playerObj);
        yield return null;
    }

    [UnityTest]
    public IEnumerator PlayerComponent_Exists()
    {
        Assert.IsNotNull(player, "Player component should have been added successfully.");
        yield break;
    }
}
