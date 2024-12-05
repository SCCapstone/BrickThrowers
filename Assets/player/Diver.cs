
/*
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Diver : Class
{
    
    public int oxygenLevel = 100;
    private bool isStunned = false;             // Whether the diver is currently stunned
    private bool isBlinded = false;             // Whether the diver is currently blinded
    private float stunTimer = 0f;
    private float blindTimer = 0f;
    

    public void Update()
    {
        
        if (isStunned)
        {
            stunTimer -= Time.deltaTime;
            if (stunTimer <= 0f)
            {
                isStunned = false;
                Debug.Log("Diver is no longer stunned.");
            }
            if (isBlinded)
            {
                blindTimer -= Time.deltaTime;
                if (blindTimer <= 0f)
                {
                    isBlinded = false;
                    Debug.Log("Diver's vision restored.");
                    // Restore diver's vision effect here
                }
            }
        }
        
    }

    
    public void Stun(float duration)
    {
        isStunned = true;
        stunTimer = duration;
        Debug.Log("Diver is stunned!");
    }

    public void Blind(float duration)
    {
        isBlinded = true;
        blindTimer = duration;
        Debug.Log("Diver is blinded by squid ink!");
        // Trigger vision-obscuring effect here, like fading screen or overlaying dark filter
    }

    public void TakeOxygenDamage(int damage)
    {
        if (isStunned) return;  // Diver takes no action while stunned
        if (isBlinded) return;  // Optional: Divers could take reduced or no actions when blinded

        oxygenLevel -= damage;
        Debug.Log("Diver's oxygen level: " + oxygenLevel);

        if (oxygenLevel <= 0)
        {
            Debug.Log("Diver has run out of oxygen!");
            // Handle game over or level restart
        }
    }
    
}
*/