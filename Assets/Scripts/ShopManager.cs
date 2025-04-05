using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private GameObject shopOverlay;

    // Start is called before the first frame update
    void Start()
    {
        shopOverlay.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
