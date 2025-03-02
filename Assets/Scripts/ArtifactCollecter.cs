using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// When a player drops artifacts in this zone, then the player will be able to collect them.
/// Keep the total value of all the artifacts collected.
/// </summary>
public class ArtifactCollecter : MonoBehaviour
{
    [SerializeField]
    private int total_value = 0;
    private ArtifactCollecter m_Collecter;
    private List<ArtifactClass> artifacts = new List<ArtifactClass>();

    /// <summary>
    /// Returns the total value of all the artifacts collected.
    /// </summary>
    public int TotalValue
    {
        get { return total_value; }
    }

    /*
     * By default, all GameObject items come with a GameItem script.
     * This script holds the ItemClass game item. But that game item could be an artifact.
     * Each ItemClass has an accessible enum type, which defines if they are a consumable, equipment, or artifact.
     *
     * The typical operation is that:
     * 1. The player drops an artifact in the ArtifactCollecter zone.
     * 2. Determine if this item is an artifact.
     * 3. Add to the total value of the artifacts.
     * 4. Add the artifact to the list of artifacts.
     *
     * Start with artifacts being added into the zone.
     */

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Item"))
        {
            // Get the GameItem script from the collision object.
            GameItem gameItem = collision.gameObject.GetComponent<GameItem>();

            // If the item is an artifact, and it has not been collected, then add it to the list of artifacts.
            if (gameItem != null)
            {
                if (gameItem.gameItem.itemType == ItemClass.ItemType.artifact)
                {
                    ArtifactClass artifact = (ArtifactClass)gameItem.gameItem;
                    total_value += artifact.GetValue();
                    artifacts.Add(artifact);
                }
            }
        }
    }
}
