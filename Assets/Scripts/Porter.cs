using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Porter : MonoBehaviour
{
    // Slots and slot manager
    [SerializeField] private GameObject slot;
    [SerializeField] private GameObject slotHolder;
    private const int MAX_SLOTS = 4;
    private const int DEFAULT_SLOTS = 3;
    private const float EXTRA_SLOT_WIDTH = 95f;
    // 95 more width to make the visual look good

    // Script enabling and disabling
    private bool enableClass = false;

    private void OnEnable()
    {
        enableClass = true;
        SetPorterInventory();

    }
    private void OnDisable()
    {
        enableClass = false;
        ResetPorterInventory();
    }

    private void SetPorterInventory()
    {
        // Set the contraint count of the slotHolder's grid layout group to the porter slots
        slotHolder.GetComponent<UnityEngine.UI.GridLayoutGroup>().constraintCount = MAX_SLOTS;
        slotHolder.GetComponent<RectTransform>().sizeDelta = new Vector2(slotHolder.GetComponent<RectTransform>().sizeDelta.x + EXTRA_SLOT_WIDTH, slotHolder.GetComponent<RectTransform>().sizeDelta.y);
        // Instantiate a slot and add it to the slotHolder as a child of the game object.
        // it should be the last child of the slotHolder
        GameObject newSlot = Instantiate(slot, slotHolder.transform);
        newSlot.name = "Porter Slot";
        newSlot.transform.localScale = Vector3.one;
        newSlot.transform.localPosition = Vector3.zero;
        newSlot.transform.localRotation = Quaternion.identity;
        newSlot.transform.SetAsLastSibling();


    }
    private void ResetPorterInventory()
    {
        // Reset all the changes from the SetPorterInventory function
        slotHolder.GetComponent<UnityEngine.UI.GridLayoutGroup>().constraintCount = DEFAULT_SLOTS;
        slotHolder.GetComponent<RectTransform>().sizeDelta = new Vector2(slotHolder.GetComponent<RectTransform>().sizeDelta.x - EXTRA_SLOT_WIDTH, slotHolder.GetComponent<RectTransform>().sizeDelta.y);
        // Remove the last slot of the slotHolder
        if (slotHolder.transform.childCount > 0)
        {
            Destroy(slotHolder.transform.GetChild(slotHolder.transform.childCount - 1).gameObject);
        }
    }

}
