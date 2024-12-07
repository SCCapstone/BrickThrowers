using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaWeed : MonoBehaviour
{
    public Collider2D specificCollider;
    public float speedReductionFactor = 0.5f;
    private Player player;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player")) {
            if (other == specificCollider) {
                player = other.gameObject.GetComponentInParent<Player>();
                if (player != null) {
                    player.speed *= speedReductionFactor;
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other == specificCollider) {
            if (player != null)
                player.speed /= speedReductionFactor;
        }

        player = null;
    }
}
