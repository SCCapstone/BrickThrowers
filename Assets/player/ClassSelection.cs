// kjthao
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ClassSelection : MonoBehaviour
{
    public Button diverButton;
    public Button harpoonButton;
    public Button porterButton;

    private string selectedClass = "Diver";

    void Start()
    {
        diverButton.onClick.AddListener(SelectDiver);
        harpoonButton.onClick.AddListener(SelectHarpooner);
        porterButton.onClick.AddListener(SelectPorter);
    }

    public void SelectDiver()
    {
        selectedClass = "Diver";
        PlayerPrefs.SetString("SelectedClass", selectedClass); // saves the selection
        Debug.Log("Selected class: Diver");
        // SpawnPlayer(diverPrefab);
    }

    public void SelectHarpooner()
    {
        selectedClass = "Harpooner";
        PlayerPrefs.SetString("SelectedClass", selectedClass); // saves the selection
        Debug.Log("Selected class: Harpooner");
        // SpawnPlayer(harpoonerPrefab);
    }

    public void SelectPorter()
    {
        selectedClass = "Porter";
        PlayerPrefs.SetString("SelectedClass", selectedClass); // saves the selection
        Debug.Log("Selected class: Porter");
        //SpawnPlayer(porterPrefab);
    }

    public string GetSelectedClass()
    {
        return selectedClass;
    }

    /*
    private void SpawnPlayer(GameObject playerPrefab)
    {
        if (currentPlayer != null) // if choosen a diff class then this will remove the previous class
        {
            Destroy(currentPlayer);
        }

        currentPlayer = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
    }
    */
}