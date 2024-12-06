// kjthao
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harpooner : Player
{
    public GameObject harpoonPrefab;  // Prefab for the harpoon

    public override void ChangeToHarpooner()
    {
        // Change sprite to Harpooner sprite
        spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/Harpooner");  // Adjust path to sprite

        // Add harpoon shooting ability
        Debug.Log("Player is now a Harpooner!");
    }

    void Update()
    {
        // Example of shooting the harpoon with the space key
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShootHarpoon();
        }
    }

    void ShootHarpoon()
    {
        if (harpoonPrefab != null)
        {
            Instantiate(harpoonPrefab, transform.position, Quaternion.identity);
            Debug.Log("Harpoon shot!");
        }
    }
}
