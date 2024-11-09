using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Porter : Player
{
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = waterGravityScale;
        rb.drag = waterDrag;
    }

    void Update()
    {
        Movement();
    }
}
