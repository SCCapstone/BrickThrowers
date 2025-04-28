// File: Assets/Tests/PlayMode/PlayerTests.cs
// To run this specific PlayMode test only:
//   • In the Unity Editor Test Runner:
//       – Window → General → Test Runner
//       – Select “PlayMode” category
//       – Right-click “PlayerTests” → Run Selected
//   • Via CLI (runs only PlayerTests):
//       Unity -batchmode -projectPath . -runTests -testPlatform PlayMode \
//         -testFilter PlayerTests -logFile -testResults TestResults/PlayerTests.xml

using NUnit.Framework;
using System.Reflection;

public class PlayerTests
{
    [Test]
    public void Player_ClassStructure_IsValid()
    {
        // Verify Player inherits from Diver
        Assert.IsTrue(typeof(Player).IsSubclassOf(typeof(Diver)), 
            "Player should inherit from Diver");

        // Verify Movement(Vector2) method exists
        var movementMethod = typeof(Player)
            .GetMethod("Movement", BindingFlags.NonPublic | BindingFlags.Instance);
        Assert.NotNull(movementMethod, 
            "Player should implement a private Movement(Vector2) method");
    }
}