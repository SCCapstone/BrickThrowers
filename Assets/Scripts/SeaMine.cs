// Copyright 2025 Brick Throwers
// SeaMine.cs - Handles the sea mine explosion and damage to the player.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaMine : MonoBehaviour {
  // public AudioClip explodeSFX;
  public AudioSource explodeSFX;
  private const float DAMAGE = 10f;

  private void OnCollisionEnter2D(Collision2D collision) {

    if (collision.gameObject.CompareTag("Player")) {
      explodeSFX.Play();
      GetComponent<CircleCollider2D>().enabled = false;
      GetComponent<SpriteRenderer>().enabled = false;
      foreach (Transform child in transform) {
        child.GetComponent<SpriteRenderer>().enabled = false;
      }
      collision.gameObject.GetComponent<Player>().TakeOxygenDamage(DAMAGE);
      Destroy(gameObject, explodeSFX.clip.length);
    }
  }

}
