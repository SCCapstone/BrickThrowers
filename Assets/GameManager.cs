using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Player player;
    // public GameObject gameOverPanel;

    /*
     * Check if the player has no health. If no health, then the player has perished and the game is over.
     * Return them to the lobby menu.
     */

    private void OnEnable()
    {
        Player.onDeath += GameOver;
    }

    private void OnDisable()
    {
        Player.onDeath -= GameOver;
    }

    public void GameOver()
    {
        if (player.GetOxygenLevel() <= 0)
        {
            // gameOverPanel.SetActive(true);
            Debug.Log("Game Over");
        }
    }
}
