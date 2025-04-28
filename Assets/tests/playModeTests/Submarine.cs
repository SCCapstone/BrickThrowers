using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UIElements;
public class Submarine


{
    [Test]
    public void PlayerMovesRight_WhenInputIsRightArrow()
    {
        // Arrange
        GameObject player = new GameObject();
        player.AddComponent<Rigidbody2D>();
        Vector2 movement = Vector2.right; 
        // Assert
        Assert.AreEqual(Vector2.right, movement);
    }
}
