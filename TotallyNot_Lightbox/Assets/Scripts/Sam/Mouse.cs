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
    public bool scrolling;
    GameObject handle;
    Transform queueParentPos;

    void Start()
    {
        queueParentPos = rtGM.slotPositions[0].transform.parent;
    }

    void Update()
    {
        this.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, 10); //Position 'Mouse' object with cursor.

        if (scrolling && Input.GetMouseButtonUp(0)) scrolling = false;
        if (scrolling)
        {
            float handleMax = 1.2f; float handleMin = -1.2f;
            float queueMax = 4.5f; float queueMin = -4.5f;
            float clampedX = Mathf.Clamp(this.transform.position.x, -1.2f, 1.2f);
            handle.transform.position = new Vector3(clampedX, handle.transform.position.y, handle.transform.position.z);

            float t = (handle.transform.position.x - handleMin) / (handleMax - handleMin);
            float queueX = Mathf.Lerp(queueMax, queueMin, t);

            queueParentPos.position = new Vector3(queueX, queueParentPos.position.y, queueParentPos.position.z);
        } //Scroll logic

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
                if (targetSlot == null  || currentParent == targetSlot) //over something invalid...
                {
                    held.transform.parent = currentParent.transform;
                    held.transform.position = currentParent.transform.position;
                } //then return Block to previous position.

                else if (targetSlot.CompareTag("PoolSlot") && (currentParent.CompareTag("Slot") || currentParent.CompareTag("SlotOccupied"))) //Over the Bin or a PoolSlot and it comes from the queue...
                {
                    print("Attempt at dropping block from queue");
                    held.transform.parent = currentParent.transform;
                    held.transform.position = currentParent.transform.position; //Temp
                }

                if (targetSlot != null && (targetSlot.CompareTag("Slot") || targetSlot.CompareTag("SlotOccupied"))) //over slot in the queue...
                {
                    if (rtGM.queue.Count < 12)
                    {
                        if (targetSlot.CompareTag("SlotOccupied")) ShiftRightUntilEmpty(targetSlot, held);
                        if (targetSlot.CompareTag("Slot")) CheckPriorSlot(targetSlot, held);
                        rtGM.PlaceIntoQueue(held, targetSlot);
                    }
                    else { }//Indication that the queue is full!

                } //then place Block into position it was dropped over

                if (!currentParent.CompareTag("PoolSlot") && held.transform.parent != currentParent.transform) currentParent.tag = "Slot";
                held = null; //and the Block is no longer held.
                currentParent = null;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (targetSlot == null && !scrolling && !collision.CompareTag("Scroll")) targetSlot = collision.gameObject;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Scroll") && Input.GetMouseButton(0) && held == null) { scrolling = true; handle = collision.gameObject; }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (targetSlot != null && !scrolling && !collision.CompareTag("Scroll")) targetSlot = null;
    }

    public void ShiftRightUntilEmpty(GameObject targetSlot, GameObject incomingBlock)
    {
        int currentIndex = rtGM.slotPositions.IndexOf(targetSlot); //Current space in queue

        while (targetSlot.CompareTag("SlotOccupied")) //'Until targetSlot is unoccupied'
        {
            GameObject displacedBlock = targetSlot.transform.GetChild(0).gameObject; //New block needed to move

            int nextIndex = currentIndex + 1;
            if (nextIndex >= rtGM.slotPositions.Count) //Error Catchall
            {
                print("No available slots to the right!");
                return;
            }

            GameObject nextSlot = rtGM.slotPositions[nextIndex]; //Next slot in queue

            ShiftRightUntilEmpty(nextSlot, displacedBlock); //Check if this is unoccupied or not too

            break;
        }

        incomingBlock.transform.position = targetSlot.transform.position;
        incomingBlock.transform.SetParent(targetSlot.transform);
        targetSlot.tag = "SlotOccupied";
    }

    public void CheckPriorSlot(GameObject targetSlot, GameObject incomingBlock)
    {
        int currentIndex = rtGM.slotPositions.IndexOf(targetSlot); //Current space in queue
        int previousIndex = 0;

        for (int i = currentIndex - 1; i >= 0; i--) //Check all previous slots
        {
            if (rtGM.slotPositions[i].CompareTag("SlotOccupied")) //If we find an occupied slot, end the loop
            {
                previousIndex = i + 1; //This means the slot 1 index after our found occupied one.
                break;
            }
        }

        GameObject insertSlot = rtGM.slotPositions[previousIndex];
        incomingBlock.transform.position = insertSlot.transform.position;
        incomingBlock.transform.SetParent(insertSlot.transform);
        insertSlot.tag = "SlotOccupied";
    }
}
