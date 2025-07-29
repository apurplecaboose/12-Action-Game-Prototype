using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Mouse : MonoBehaviour
{
    public gameManager rtGM;
    public GameObject targetSlot;
    public GameObject held;
    public GameObject currentParent;

    void Start()
    {

    }

    void Update()
    {
        this.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, 10); //Position 'Mouse' object with cursor.

        if (Input.GetMouseButton(0)) //If clicking...
        {
            if (targetSlot != null && targetSlot.transform.childCount != 0 && held == null) //and not already holding anything...
            {
                currentParent = targetSlot;
                held = targetSlot.transform.GetChild(0).gameObject; //pick up the Block.
            }
        }

        if (held != null) //If holding a block...
        {
            held.transform.parent = this.transform; //Block moves with mouse.

            if (Input.GetMouseButtonUp(0)) //If Block released...
            {
                if (targetSlot == null || targetSlot.CompareTag("SlotOccupied") || targetSlot.CompareTag("PoolSlot")) //over nothing...
                {
                    if (currentParent.CompareTag("PoolSlot")) //and comes from the pool...
                    {
                        held.transform.parent = currentParent.transform;
                        held.transform.position = currentParent.transform.position;
                    }//then return Block to previous position.

                    if (currentParent.CompareTag("SlotOccupied"))//and comes from the queue...
                    {
                        held.transform.parent = currentParent.transform;
                        held.transform.position = currentParent.transform.position; //CHANGE FROM (return to currentParent) --> (return to correct Pool).
                    }//then return to the pool.
                } 

                if (targetSlot != null && targetSlot.CompareTag("Slot")) //over unoccupied slot...
                {
                    held.transform.parent = targetSlot.transform;
                    held.transform.position = targetSlot.transform.position;
                    targetSlot.tag = "SlotOccupied";
                } //then place Block into position it was dropped over **REMEMBER TO DISPLACE TILE OCCUPIER**

                if (!currentParent.CompareTag("PoolSlot") && held.transform.parent != currentParent.transform) currentParent.tag = "Slot";
                held = null; //and the Block is no longer held.
                currentParent = null;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (targetSlot == null) targetSlot = collision.gameObject;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (targetSlot != null) targetSlot = null;
    }
}
