using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class OctopusPTest
{
    private GameObject octoObj;
    private Octopus octo;
    private Rigidbody2D rb;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        octoObj = new GameObject("Octopus");
        rb      = octoObj.AddComponent<Rigidbody2D>();
        octo    = octoObj.AddComponent<Octopus>();

        // Inject the Rigidbody2D so the private Roam() sets its velocity
        typeof(Octopus)
            .GetField("rb", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(octo, rb);

        yield return null;
    }

    [UnityTearDown]
    public IEnumerator TearDown()
    {
        Object.Destroy(octoObj);
        yield return null;
    }

    [UnityTest]
    public IEnumerator Octopus_Roam_SetsVelocityMagnitude()
    {
        // On the first Update(), StartCoroutine(Roam()) will run
        yield return null;

        Assert.AreEqual(octo.moveSpeed, rb.velocity.magnitude, 0.1f,
            "After one frame, octopus should be roaming at moveSpeed");
    }
}
