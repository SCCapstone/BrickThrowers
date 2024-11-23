// kjthao 2024
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    
    public void SetVolume (float volume)
    {
        //Debug.Log(volume); // checks in console when slider moves!
        audioMixer.SetFloat("volume", volume); // must be named correctly here and in unity (exposed param.)
    }
}
