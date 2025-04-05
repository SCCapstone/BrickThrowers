using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    private string selectedClass;

    void Start()
    {
        selectedClass = "None";
    }

    public void SelectHarpooner()
    {
        selectedClass = "Harpooner";
        Debug.Log("Harpooner selected");
    }

    public void SelectPorter()
    {
        selectedClass = "Porter";
        Debug.Log("Porter selected");
    }

    public string GetSelectedClass()
    {
        return selectedClass;
    }

    public void ExitClassSelection()
    {
        PlayerPrefs.SetString("PlayerClass", selectedClass);
        PlayerPrefs.Save();

        Debug.Log("Class saved: " + selectedClass);

        gameObject.SetActive(false);
    }

}
