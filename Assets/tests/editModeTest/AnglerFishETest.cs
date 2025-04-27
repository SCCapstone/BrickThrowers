using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

[TestFixture]

public class AnglerFishETest 
{
       // Verifies that the Anglerfish’s public defaults are as expected.
    [Test]
    public void DefaultValuesAreCorrect()
    {
        var go = new GameObject();
        var fish = go.AddComponent<Anglerfish>();
        Assert.AreEqual(2f, fish.swimSpeed, "swimSpeed should default to 2f");
        Assert.AreEqual(5f, fish.directionChangeInterval, "directionChangeInterval should default to 5f");
        Assert.AreEqual(3f, fish.lightIntensity, "lightIntensity should default to 3f");
        Assert.AreEqual(5f, fish.detectionRange, "detectionRange should default to 5f");
        Assert.AreEqual(0.1f, fish.oxygenDamage, "oxygenDamage should default to 0.1f");
        Object.DestroyImmediate(go);
    }

    // Checks that after one frame, the fish’s Rigidbody2D has been set moving at swimSpeed.
    [UnityTest]
    public IEnumerator SwimMovementOccurs()
    {
        var go = new GameObject();
        var fish = go.AddComponent<Anglerfish>();
        var rb = go.AddComponent<Rigidbody2D>();

        // Place a player outside detectionRange so InteractWithPlayer won't fire
        var player = new GameObject { tag = "Player" };
        player.transform.position = Vector3.one * (fish.detectionRange + 1f);

        // Let Start() and Update() run
        yield return null;

        float speed = rb.velocity.magnitude;
        Assert.AreEqual(fish.swimSpeed, speed, 0.1f, "After one frame, velocity magnitude should equal swimSpeed");

        Object.Destroy(go);
        Object.Destroy(player);
    }
}
