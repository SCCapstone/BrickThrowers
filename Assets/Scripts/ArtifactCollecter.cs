using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactCollecter : MonoBehaviour
{
    /*
     * When artifacts are dropped in the drop zone, the ArtifactCollecter should keep the tally of the total value.
     * When an artifact is dropped in this zone, the script should take the value of the artifact and add it to the total value.
     * First, we need to determine whether the object that is dropped in the zone is an artifact.
     * I imagine that the artifacts will have a tag called "Artifact".
     * Remember that when artifacts are dropped, it is a new instance of the artifact.
     * I want to check only once for the artifact, I need not do it more.
     * I need to keep the value somewhere.
     */


    // Actions
    public static event Action<bool> onPlayerCollectArtifact;

    // Variables
    public static int totalValue = 0; // Start with total value at a scene.

    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("value = " + totalValue);
        if (collision.gameObject.CompareTag("Item"))
        {
            // Each Game Item game object has a variable named gameItem that is an instance of the ItemClass script.
            // Determine if this variable is a type of ArtifactClass. This can be done by checking the Item Type enum and if it is stated as an Artifact.
            // If it is, cast the gameItem variable to an ArtifactClass.
            // If not, ignore this operation.

            if (
                collision.gameObject.GetComponent<GameItem>().gameItem.itemType == ItemClass.ItemType.artifact
            )
            {
                ArtifactClass artifact = (ArtifactClass) collision.gameObject.GetComponent<GameItem>().gameItem;
                if (!artifact.inDropZone)
                {
                    totalValue += artifact.value;
                    artifact.inDropZone = true;
                }
            }
            else
            {
                return;
            }
        }
    }

    

}
