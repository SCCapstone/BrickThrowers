using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harpooner : Player
{
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = waterGravityScale;
        rb.drag = waterDrag;
        // Harpooner does not have less health.
    }

    void Update()
    {
        Movement();
    }
}
