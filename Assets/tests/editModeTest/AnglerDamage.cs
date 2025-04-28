/*KJTHAO*/
using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

// test class is for making sure the anglerifish can actually take damage
public class AnglerDamage
{
    private GameObject anglerfishObject;
    private Anglerfish anglerfish;

    [SetUp]
    public void Setup()
    {
        // spawns anglerfish, gives rigi body etc.
        anglerfishObject = new GameObject();
        anglerfish = anglerfishObject.AddComponent<Anglerfish>();

        anglerfishObject.AddComponent<Rigidbody2D>();

        anglerfish.health = 30;
    }

    [TearDown]
    public void Teardown()
    {   
        // needs teardown so it doesnt hurt the other tests
        if (anglerfishObject != null)
        {
            Object.DestroyImmediate(anglerfishObject);
        }
    }

    [Test]
    public void Anglerfish_TakesDamage_ReducesHealth()
    {
        // slap the fish with harpooner to deal damage
        int startingHealth = anglerfish.health;
        int damage = 10;

        anglerfish.TakeDamage(damage);

        Assert.AreEqual(startingHealth - damage, anglerfish.health, "Anglerfish did not take the correct amount of damage.");
    }
}
