
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public abstract class Diver : MonoBehaviour
{
    public float oxygenLevel = 100;
    private bool isStunned = false;             // Whether the diver is currently stunned
    private bool isBlinded = false;             // Whether the diver is currently blinded
    private float stunTimer = 0f;
    private float blindTimer = 0f;

    public static event Action onDeath; // Signal that the diver has died

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
    public void TakeOxygenDamage(float damage)
    {
        if (isStunned) return;  // Diver takes no action while stunned
        if (isBlinded) return;  // Optional: Divers could take reduced or no actions when blinded
        if (GodModeStatus()) return; // Diver takes no damage if in god mode

        oxygenLevel -= damage;
        Debug.Log("Diver's oxygen level: " + oxygenLevel);

        if (oxygenLevel <= 0)
        {
            Debug.Log("Diver has run out of oxygen!");
            onDeath?.Invoke();

        }
    }

    // Determines if the diver is in god mode.
    // All pirates should always have this set to false.
    // Player is the only one that can toggle this.
    public abstract bool GodModeStatus();
}
