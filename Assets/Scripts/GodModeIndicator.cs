using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GodModeIndicator : MonoBehaviour
{
    [SerializeField] private GameObject godModeIndicatorPrefab;
    private static bool isGodModeActive = false;
    private string lobbySceneName;
    private string activeSceneName;

    private void Start()
    {
        lobbySceneName = SceneManager.GetActiveScene().name;
    }
    // Start is called before the first frame update
    void Awake()
    {
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
        if (activeSceneName == lobbySceneName)
        {
            Debug.Log($"Scene changed to {activeSceneName}. God mode status: {isGodModeActive}");   
        }
        else
        {
            Debug.Log($"Scene changed to {activeSceneName}. God mode status: {isGodModeActive}");
        }



    }
}
