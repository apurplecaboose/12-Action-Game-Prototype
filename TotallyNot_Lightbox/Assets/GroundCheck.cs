using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    int triggerCount;
    public bool isGrounded;

    public void Update()
    {
        if (triggerCount > 0) isGrounded = true;
        else isGrounded = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Floor")) triggerCount++;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Floor")) triggerCount--;
    }
}
