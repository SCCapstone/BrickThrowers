// kjthao
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.Universal;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyMenu : MonoBehaviour
{
    private LobbyCountdown lobbyCountdown;

    private void Start()
    {
        lobbyCountdown = FindObjectOfType<LobbyCountdown>();
    }
    
    public void RunGame(int sceneIndexNum)
    {
        lobbyCountdown.StartCountdown(sceneIndexNum);
        
    }
}
