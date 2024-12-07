using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaMine : MonoBehaviour
{
    // public AudioClip explodeSFX;
    public AudioSource explodeSFX;
    public Collider2D specificCollider;
    private bool hasCollided = false;

    void OnCollisionEnter2D(Collision2D other)
    {
        if(hasCollided)
            return;

        if (other.gameObject.CompareTag("Player")) {
            if (other.collider == specificCollider) {
                hasCollided = true;
                explodeSFX.Play();
                Destroy(gameObject, explodeSFX.clip.length-5);
            }
        }
    }

}
