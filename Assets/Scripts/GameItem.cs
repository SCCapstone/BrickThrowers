/*
 * Copyright 2025 Scott Do
 * 2/15/2025
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameItem : MonoBehaviour
{
    // The type of item associated with every collectable that appears in the game.
    public ItemClass gameItemClass;

    /*
     * I need a private variable for if an artifact in particular is in a drop zone.
     * By default, all items that are NOT artifacts are not in the drop zone.
     */
    private bool inDropZone = false;

    public bool InDropZone
    {
        get
        {
            return inDropZone;
        }
        set
        {
            if (gameItemClass == null)
            {
                Debug.LogWarning("gameItemClass is null, cannot set InDropZone.");
                return;
            }
            if (gameItemClass.itemType == ItemClass.ItemType.artifact)
            {
                inDropZone = value;
            }
            else
            {
                Debug.LogWarning("Attempted to set InDropZone on a non-artifact item.");
            }
        }

    }

}
