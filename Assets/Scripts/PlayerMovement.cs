// kjthao
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Animator animator;

    private void Update()
    {
        // need to get the player's horizontal and vertical input
        float horizontal = Input.GetAxis("Horizontal"); 
        float vertical = Input.GetAxis("Vertical");

        // swimmming or standing
        bool isSwimming = horizontal != 0 || vertical != 0;
        bool isStanding = !isSwimming;

        animator.SetBool("isSwimming", isSwimming);
        animator.SetBool("isStanding", isStanding);

        // directions
        if (isSwimming)
        {
            if (horizontal > 0)
            {
                animator.SetFloat("direction", 1);
            }
            else if (horizontal < 0)
            {
                animator.SetFloat("direction", -1);
            }
            else if (vertical != 0)
            {
                animator.SetFloat("direction", 0);
            }
        }
    }
}