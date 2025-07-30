using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rtRB;
    public float jumpForce;
    public float moveSpeed;

    void Start()
    {
        rtRB = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Space)) && GetComponent<GroundCheck>().isGrounded) //Jump
        {
            rtRB.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        }

        if (Mathf.Abs(rtRB.velocity.x) < 20f)
        {
            if (Input.GetKey(KeyCode.A)) //Left
            {
                rtRB.AddForce(new Vector2(-moveSpeed, 0));
            }
            if (Input.GetKey(KeyCode.D)) //Right
            {
                rtRB.AddForce(new Vector2(moveSpeed, 0));
            }
        }
    }
}
