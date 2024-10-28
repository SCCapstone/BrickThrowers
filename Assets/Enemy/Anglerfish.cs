using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anglerfish : Enemy
{
    public Light anglerLight; // to show in the components tab (reminder to self- to assign it later)
    private float swimSpeed = 4f; // giving the anglerfish a custom swim speed
    public float lightIntensity = 3f;

    void Start()
    {
        if (anglerLight != null)
        {
            anglerLight.intensity = lightIntensity; // set as the 3f intensity.
        }
        
    }

    void Update()
    {
        Move();
        Flicker();
        
    }

    private void Flicker()
    {
        if (anglerLight != null)
        {
            anglerLight.intensity = Random.Range(0.5f, lightIntensity); // changes the intensity to .5, giving a flicker effect
        }
    }

    // add attack method and other behaviors here!
}
