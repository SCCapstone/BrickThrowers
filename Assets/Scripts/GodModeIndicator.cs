using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodModeIndicator : MonoBehaviour
{
    [SerializeField] private GameObject godModeIndicatorPrefab;
    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    
}
