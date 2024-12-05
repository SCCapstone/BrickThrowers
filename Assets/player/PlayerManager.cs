// kjthao
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public GameObject diverPrefab;
    public GameObject harpoonerPrefab;
    public GameObject porterPrefab;

    void Start()
    {
        string selectedClass = PlayerPrefs.GetString("SelectedClass", "Diver"); // since diver is the default/regular class

        if (selectedClass == "Diver")
        {
            Instantiate(diverPrefab, Vector3.zero, Quaternion.identity);
        }
        else if (selectedClass == "Harpooner")
        {
            Instantiate(harpoonerPrefab, Vector3.zero, Quaternion.identity);
        }
        else if (selectedClass == "Porter")
        {
            Instantiate(porterPrefab, Vector2.zero, Quaternion.identity);
        }
    }
}