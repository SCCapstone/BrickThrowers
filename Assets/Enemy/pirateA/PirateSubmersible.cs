using UnityEngine;

public class PirateSubmersible : MonoBehaviour
{
    public GameObject piratePrefab;    // Prefab of pirate divers
    public int health = 100;          // Health of the submersible
    public float deployInterval = 5f; // Time between deploying pirates

    private float deployTimer;

    void Update()
    {
        deployTimer -= Time.deltaTime;
        if (deployTimer <= 0f)
        {
            DeployPirate();
            deployTimer = deployInterval;
        }
    }

    void DeployPirate()
    {
        GameObject pirate = Instantiate(piratePrefab, transform.position, Quaternion.identity);
        Debug.Log("Pirate deployed!");
    }

    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;
        if (health <= 0)
        {
            Debug.Log("Pirate submersible destroyed!");
            Destroy(gameObject);
        }
    }
}
