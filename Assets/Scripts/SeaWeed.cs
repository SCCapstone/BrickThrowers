using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaWeed : MonoBehaviour
{
    public static event Action<float> onPlayerSlowedDown;
    public static event Action onPlayerSpeedRestored;

    private const float REDUCED_SPEED = 20f;

    // Triggers if player enteres the sea weed
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player slowed down");
            onPlayerSlowedDown?.Invoke(REDUCED_SPEED);
        }
    }

    // Triggers if player exits the sea weed
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player speed restored");
            onPlayerSpeedRestored?.Invoke();
        }
    }
}
