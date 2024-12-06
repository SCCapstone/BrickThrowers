// kjthao
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassSelection : MonoBehaviour
{
    public Player player;  // Reference to the player

    // Call this method when the "Harpooner" button is clicked
    public void SelectHarpooner()
    {
        Harpooner harpooner = player.GetComponent<Harpooner>();
        if (harpooner != null)
        {
            harpooner.ChangeToHarpooner();  // Switch to Harpooner
        }
    }

    // Call this method when the "Porter" button is clicked
    public void SelectPorter()
    {
        Porter porter = player.GetComponent<Porter>();
        if (porter != null)
        {
            porter.ChangeToPorter();  // Switch to Porter
        }
    }
}
