using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
   /*
        * A Enemy Class is a parent class of all enemies in the game
        * As the parent class it will be the have the main basic
        components that each enemy must have when they are in the game
        * Due to this game being 2D I will also have to adapt the movement
        of the enemies on the XY plain and make them focus on the player
        * Enemies should have health, speed, and attack as the basics
        for all of them
   */

   private float speed = 6f;

   private Rigidbody2D rigidbody;
   private Vector2 movement;

    // How much damage for player
   private int damage = 4;
    // health for enemies
   private int health = 5;


}
