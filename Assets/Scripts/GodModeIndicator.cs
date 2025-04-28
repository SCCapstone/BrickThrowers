using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GodModeIndicator : MonoBehaviour
{
    // Game objects
    [SerializeField] private GameObject godModeIndicatorPrefab;

    // Activation
    private static bool isGodModeActive = false;

    // Scene names
    private static string lobbySceneName;
    private string activeSceneName;
    
    // Actions
    public static event System.Action onGodModeActivated;

    void Awake()
    {
        lobbySceneName = SceneManager.GetActiveScene().name;
        DontDestroyOnLoad(this.gameObject);
    }

    private void OnEnable()
    {
        PauseMenu.onGodModeActivated += ToggleGodModeIndicator;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        PauseMenu.onGodModeActivated -= ToggleGodModeIndicator;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    private void ToggleGodModeIndicator(bool buttonStatus)
    { 
        isGodModeActive = buttonStatus;
        if (isGodModeActive)
            Debug.Log($"Action recieved. God mode activated. Status: {isGodModeActive}");
        else
            Debug.Log($"Action recieved. God mode deactivated. Status: {isGodModeActive}");
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Get the name of the current active scene.
        activeSceneName = SceneManager.GetActiveScene().name;

        // Check if the current scene is the lobby scene. For now, leave debug statements.
        // Do not destroy or change or create any activity, only a debug statement notifying that the scene has changed.
        if (activeSceneName != lobbySceneName)
        {
            Debug.Log($"Scene changed to {activeSceneName}. God mode status: {isGodModeActive}");
            onGodModeActivated?.Invoke();
        }
        else
        {
            Debug.Log($"Hooray! You are in the {activeSceneName} which should be the {lobbySceneName}. God mode status: {isGodModeActive}");
        }



    }
}
