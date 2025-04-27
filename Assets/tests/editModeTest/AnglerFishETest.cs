using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

[TestFixture]
public class AnglerFishETest 
{
    [Test]
    public void DefaultValuesAreCorrect()
    {
        var go = new GameObject("Anglerfish");
        var fish = go.AddComponent<Anglerfish>();

        Assert.AreEqual(2f,    fish.swimSpeed,                "swimSpeed should default to 2f");
        Assert.AreEqual(5f,    fish.directionChangeInterval, "directionChangeInterval should default to 5f");
        Assert.AreEqual(3f,    fish.lightIntensity,         "lightIntensity should default to 3f");
        Assert.AreEqual(5f,    fish.detectionRange,         "detectionRange should default to 5f");
        Assert.AreEqual(0.2f,  fish.oxygenDamage,           "oxygenDamage should default to 0.2f");
        Assert.AreEqual(30,    fish.health,                 "health should default to 30");

        Object.DestroyImmediate(go);
    }

    [UnityTest]
    public IEnumerator SwimMovementOccurs()
    {
        var go = new GameObject("Anglerfish");
        var fish = go.AddComponent<Anglerfish>();
        var rb = go.AddComponent<Rigidbody2D>();

        // Ensure there's a "Player" so Start() finds one,
        // but place it out of any collision range.
        var player = new GameObject("Player");
        player.tag = "Player";
        player.transform.position = Vector3.one * (fish.detectionRange + 1f);

        // Wait one frame to let Awake(), Start(), and Update() run
        yield return null;

        float speed = rb.velocity.magnitude;
        Assert.AreEqual(fish.swimSpeed, speed, 0.1f,
            "After one frame, velocity magnitude should equal swimSpeed");

        Object.DestroyImmediate(go);
        Object.DestroyImmediate(player);
    }
}
