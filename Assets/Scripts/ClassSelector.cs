using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ClassSelector : MonoBehaviour
{
    public GameObject player;
    public RuntimeAnimatorController diverAnimatorController;
    public RuntimeAnimatorController harpoonerAnimatorController;
    public RuntimeAnimatorController porterAnimatorController;

    private Animator playerAnimator;

    public Button diverButton;
    public Button harpoonerButton;
    public Button porterButton;

    public TextMeshProUGUI classIndicatorText;
    private string selectedClass;

    // New fields for audio
    public AudioClip classSelectionSound;  // Drag your sound clip here in the Inspector
    private AudioSource audioSource;

    void Start()
    {
        playerAnimator = player.transform.GetChild(3).GetComponent<Animator>();

        // Initialize the AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        diverButton.onClick.AddListener(SelectDiver);
        harpoonerButton.onClick.AddListener(SelectHarpooner);
        porterButton.onClick.AddListener(SelectPorter);
    }

    private void DisableAllClassScripts()
    {
        //var diver = player.GetComponent<Diver>();
        //if (diver != null) diver.enabled = false;

        var harpooner = player.GetComponent<Harpooner>();
        if (harpooner != null) harpooner.enabled = false;

        var porter = player.GetComponent<Porter>();
        if (porter != null) porter.enabled = false;
    }

    public void SelectDiver()
    {
        DisableAllClassScripts();

        playerAnimator.runtimeAnimatorController = diverAnimatorController;
        selectedClass = "Diver";
        ClassSelectionData.SelectedClass = selectedClass;

        var diver = player.GetComponent<Diver>();
        if (diver != null) diver.enabled = true;

        UpdateClassIndicator();
    }

    public void SelectHarpooner()
    {
        DisableAllClassScripts();

        playerAnimator.runtimeAnimatorController = harpoonerAnimatorController;
        selectedClass = "Harpooner";
        ClassSelectionData.SelectedClass = selectedClass;

        var harpooner = player.GetComponent<Harpooner>();
        if (harpooner != null) harpooner.enabled = true;

        UpdateClassIndicator();
    }

    public void SelectPorter()
    {
        DisableAllClassScripts();

        playerAnimator.runtimeAnimatorController = porterAnimatorController;
        selectedClass = "Porter";
        ClassSelectionData.SelectedClass = selectedClass;

        var porter = player.GetComponent<Porter>();
        if (porter != null) porter.enabled = true;

        UpdateClassIndicator();
    }

    private void UpdateClassIndicator()
    {
        classIndicatorText.text = $"You have chosen {selectedClass}!";
        classIndicatorText.color = Color.green;

        // Play the sound when the class is updated
        if (audioSource != null && classSelectionSound != null)
        {
            audioSource.PlayOneShot(classSelectionSound);
        }
    }
}
