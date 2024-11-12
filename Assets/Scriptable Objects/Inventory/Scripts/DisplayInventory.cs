using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayInventory : MonoBehaviour

{
    public InventoryObject inventory;
    public int X_SPACE_BETWEEN_ITEM;
    Dictionary<InventorySlot, GameObject> itemDisplayed = new Dictionary<InventorySlot, GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        CreateDisplay();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateDisplay();
    }

    public void CreateDisplay()
    {
        for (int i = 0; i < inventory.Container.Count; i++)
        {
            var obj = inventory.Container[i].item.prefab;
            var go = Instantiate(obj, Vector3.zero, Quaternion.identity, transform);
            go.GetComponent<RectTransform>().localPosition = GetPosition(i);
            go.GetComponentInChildren<UnityEngine.UI.Text>().text = inventory.Container[i].amount.ToString("n0");
            itemDisplayed.Add(inventory.Container[i], go);
        }
    }
