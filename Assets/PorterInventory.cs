using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PorterInventory : MonoBehaviour
{
    public GameObject porterExtraSlot;

    void Start()
    {
        if (porterExtraSlot != null)
        {
            porterExtraSlot.SetActive(false);
        }

        // this checks if the player is a Porter and enable extra slot
        if (GetComponent<Porter>() != null) 
        {
            ActivateExtraSlot();
        }
    }

    void ActivateExtraSlot()
    {
        if (porterExtraSlot != null)
        {
            porterExtraSlot.SetActive(true);
        }
    }
}
