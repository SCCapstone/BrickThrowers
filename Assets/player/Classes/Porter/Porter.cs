// kjthao
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Porter : Player
{
    private int extraInventorySlots = 1; // 1 extra slot given - which can be changed but set to 1 since inventory system hasnt been implemented yet

    void Start()
    {
        AddExtraInventorySlots();
    }

    private void AddExtraInventorySlots()
    {
        // just a placeholder until the inventory system is complete, this is where to put the logic to interact w/ the system
        Debug.Log($"Poster gains {extraInventorySlots} extra inventory slots.");
    }
}