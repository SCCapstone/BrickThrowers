// kjthao
/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harpooner : Player
{
    private GameObject harpoonPrefab;
    private float harpoonSpeed = 10f; // attack speed/ thrown speed

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
        }
    }

    private void Attack()
    {
        if (harpoonPrefab != null)
        {
            Vector3 spawnPosition = transform.position + transform.right; // need to spawn the harpoon close to player first
            GameObject harpoon = Instantiate(harpoonPrefab, spawnPosition, Quaternion.identity);
            Rigidbody2D harpoonRb = harpoon.GetComponent<Rigidbody2D>();

            if (harpoonRb != null)
            {
                harpoonRb.velocity = transform.right * harpoonSpeed; // this basically throws the harpon in the direct. of where the player is facing!
            }

        }
    }
}
*/