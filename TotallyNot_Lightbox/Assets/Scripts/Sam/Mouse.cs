using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Mouse : MonoBehaviour
{
    public string SceneString;
    public gameManager rtGM;
    public PoolSlotManager rtPSM;
    public GameObject destinationSlot;
    public GameObject held;
    public GameObject originSlot;
    GameObject handle;
    GameObject trueDestinationSlot;
    int destinationIndex;
    Transform queueParentPos;

    //Considerations:
    // - Limited Visual Feedback
    // - Made need to shrink all elements for level view
    // - Dependent on Tags (Need prefabs to be tagged)
    

    void Start()
    {
        queueParentPos = rtGM.slotPositions[0].transform.parent;
    }

    public void RightShiftButton()
    {
        if (queueParentPos.transform.position.x >= -4.5f) queueParentPos.transform.position -= new Vector3(0.75f, 0, 0);
    }
    public void LeftShiftButton()
    {
        if (queueParentPos.transform.position.x <= 4.5f) queueParentPos.transform.position += new Vector3(0.75f, 0, 0);
    }


    void Update()
    {
        this.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, 10); //Position 'Mouse' object with cursor.

        if (Input.GetMouseButtonDown(0)) //If clicking...
        {
            if (destinationSlot != null && destinationSlot.CompareTag("Continue") && rtGM.Queue.Count > 0)
            {
                SceneManager.LoadScene(SceneString);
            }

            else if (destinationSlot != null && held == null) //and not already holding anything...
            {
                originSlot = destinationSlot;
                if (destinationSlot.CompareTag("PoolSlot") && destinationSlot.transform.childCount > 1) held = destinationSlot.transform.GetChild(1).gameObject; //pick up the Block.
                else if (destinationSlot.CompareTag("SlotOccupied")) { held = destinationSlot.transform.GetChild(0).gameObject; }
                if (held != null)
                {
                    held.transform.parent = this.transform;
                    held.GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.None;
                    if (held.transform.childCount != 0) held.transform.GetChild(0).GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.None;
                    //held.transform.localScale = new Vector3(2f, 2f, 0);
                }

                if (originSlot.CompareTag("SlotOccupied"))
                {
                    RemoveGapInQueue(originSlot);
                }
            }
        }

        if (Input.GetMouseButtonUp(0) && held != null) //If held block released...
        {
            if (originSlot.CompareTag("PoolSlot")) //comes from the pool...
            {
                if (destinationSlot == null)
                {
                    held.transform.parent = originSlot.transform;
                    held.transform.position = originSlot.transform.position;
                }//send block back to where it came from.

                else if (destinationSlot.CompareTag("Slot"))
                {
                    destinationIndex = rtGM.slotPositions.IndexOf(destinationSlot);
                    int testedIndex = destinationIndex;
                    for (int i = 0; i <= testedIndex; i++)
                    {
                        if (rtGM.slotPositions[i].CompareTag("Slot"))
                        {
                            testedIndex = i;
                            break;
                        }
                    }
                    trueDestinationSlot = rtGM.slotPositions[testedIndex];

                    PlaceBlock(held, trueDestinationSlot, originSlot);
                } //goes to an empty queue slot. Add it to the earliest queue slot.

                else if (destinationSlot.CompareTag("SlotOccupied"))
                {
                    if (rtGM.Queue.Count < 12)
                    {
                        destinationIndex = rtGM.slotPositions.IndexOf(destinationSlot);

                        int emptyIndex = -1;
                        for (int i = destinationIndex + 1; i < rtGM.slotPositions.Count; i++)
                        {
                            if (rtGM.slotPositions[i].CompareTag("Slot"))
                            {
                                emptyIndex = i;
                                break;
                            }
                        }
                        if (emptyIndex == -1)
                        {
                            print("No available slot to displace into.");
                            PlaceBlock(held, originSlot, originSlot);
                            return;
                        }

                        for (int i = emptyIndex; i > destinationIndex; i--)
                        {
                            GameObject blockToMove = rtGM.slotPositions[i - 1].transform.GetChild(0).gameObject;
                            rtGM.Queue.RemoveAt(i - 1);
                            PlaceBlock(blockToMove, rtGM.slotPositions[i], rtGM.slotPositions[i - 1]);
                        }

                        trueDestinationSlot = rtGM.slotPositions[destinationIndex];
                        PlaceBlock(held, trueDestinationSlot, originSlot);
                    }
                    else { } //Indication that the queue is full, either remove some blocks or continue to execution.

                }//goes to an occupied queue slot. Move all later blocks in queue down and place block into chosen slot.

                else if (destinationSlot.CompareTag("PoolSlot") || destinationSlot.CompareTag("Continue"))
                {
                    held.transform.parent = originSlot.transform;
                    held.transform.position = originSlot.transform.position;
                }//Simply drop the block, and have it return to where it was picked up from.
            }

            else if (originSlot.CompareTag("SlotOccupied") || originSlot.CompareTag("Slot")) //comes from the queue...
            {
                if (destinationSlot == null || destinationSlot == originSlot || destinationSlot.CompareTag("Continue"))
                {
                    if (originSlot.CompareTag("Slot"))
                    {
                        destinationIndex = rtGM.slotPositions.IndexOf(originSlot);
                        int testedIndex = destinationIndex;
                        for (int i = 0; i <= testedIndex; i++)
                        {
                            if (rtGM.slotPositions[i].CompareTag("Slot"))
                            {
                                testedIndex = i;
                                break;
                            }
                        }
                        trueDestinationSlot = rtGM.slotPositions[testedIndex];

                        PlaceBlock(held, trueDestinationSlot, originSlot);
                    }
                    else if (originSlot.CompareTag("SlotOccupied"))
                    {
                        destinationIndex = rtGM.slotPositions.IndexOf(originSlot);

                        int emptyIndex = -1;
                        for (int i = destinationIndex + 1; i < rtGM.slotPositions.Count; i++)
                        {
                            if (rtGM.slotPositions[i].CompareTag("Slot"))
                            {
                                emptyIndex = i;
                                break;
                            }
                        }
                        if (emptyIndex == -1)
                        {
                            print("No available slot to displace into.");
                            PlaceBlock(held, originSlot, originSlot);
                            return;
                        }

                        for (int i = emptyIndex; i > destinationIndex; i--)
                        {
                            GameObject blockToMove = rtGM.slotPositions[i - 1].transform.GetChild(0).gameObject;
                            rtGM.Queue.RemoveAt(i - 1);
                            PlaceBlock(blockToMove, rtGM.slotPositions[i], rtGM.slotPositions[i - 1]);
                        }

                        trueDestinationSlot = rtGM.slotPositions[destinationIndex];
                        PlaceBlock(held, trueDestinationSlot, originSlot);
                    }
                } //undo RemoveFromGap, and place held block back where it came from.

                else if (destinationSlot.CompareTag("PoolSlot"))
                {
                    GameObject targetPoolSlot = null;
                    switch (held.tag)
                    {
                        case "Left":
                            targetPoolSlot = rtPSM.pool1;
                            break;
                        case "Right":
                            targetPoolSlot = rtPSM.pool2;
                            break;
                        case "LeftJump":
                            targetPoolSlot = rtPSM.pool3;
                            break;
                        case "RightJump":
                            targetPoolSlot = rtPSM.pool4;
                            break;
                        default:
                            print("Block tag not recognized for pool return: " + held.tag);
                            break;
                    }

                    if (targetPoolSlot != null)
                    {
                        held.transform.SetParent(targetPoolSlot.transform);
                        held.transform.position = targetPoolSlot.transform.position;
                    }
                } //sends held block back to the correct pool.

                else if (destinationSlot.CompareTag("Slot"))
                {
                    destinationIndex = rtGM.slotPositions.IndexOf(destinationSlot);
                    int testedIndex = destinationIndex;
                    for (int i = 0; i <= testedIndex; i++)
                    {
                        if (rtGM.slotPositions[i].CompareTag("Slot"))
                        {
                            testedIndex = i;
                            break;
                        }
                    }
                    trueDestinationSlot = rtGM.slotPositions[testedIndex];

                    PlaceBlock(held, trueDestinationSlot, originSlot);
                } //Let all other blocks in queue move left until all other blocks are settled, then check if any slots to the left of this slot are empty. If so, place on the earliest available slot, if not, place where attempted.

                else if (destinationSlot.CompareTag("SlotOccupied"))
                {
                    destinationIndex = rtGM.slotPositions.IndexOf(destinationSlot);

                    int emptyIndex = -1;
                    for (int i = destinationIndex + 1; i < rtGM.slotPositions.Count; i++)
                    {
                        if (rtGM.slotPositions[i].CompareTag("Slot"))
                        {
                            emptyIndex = i;
                            break;
                        }
                    }
                    if (emptyIndex == -1)
                    {
                        print("No available slot to displace into.");
                        PlaceBlock(held, originSlot, originSlot);
                        return;
                    }

                    for (int i = emptyIndex; i > destinationIndex; i--)
                    {
                        GameObject blockToMove = rtGM.slotPositions[i - 1].transform.GetChild(0).gameObject;
                        rtGM.Queue.RemoveAt(i - 1);
                        PlaceBlock(blockToMove, rtGM.slotPositions[i], rtGM.slotPositions[i - 1]);
                    }

                    trueDestinationSlot = rtGM.slotPositions[destinationIndex];
                    PlaceBlock(held, trueDestinationSlot, originSlot);
                } //Let all other blocks in queue move left until all other blocks are settled, then see if attempted slot is still occupied. If so, push the occupying block and any blocks behind it +1 to the right and place block, if not, simply place block.
            }

            held = null; rtPSM.UpdateText();
        }
    }

    public void PlaceBlock(GameObject block, GameObject slot, GameObject origin) //Visually adds the block to queue and calls AddToQueue.
    {
        block.transform.parent = slot.transform;
        block.transform.position = slot.transform.position;

        if (slot.CompareTag("Slot") || slot.CompareTag("SlotOccupied"))
        {
            block.GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
            if (block.transform.childCount != 0) block.transform.GetChild(0).GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
            if (slot.CompareTag("Slot")) slot.tag = "SlotOccupied";
        }
        else if (slot.CompareTag("PoolSlot"))
        {
            block.GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.None;
            if (block.transform.childCount != 0) block.transform.GetChild(0).GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.None;
            if (origin.CompareTag("SlotOccupied")) origin.tag = "Slot";
        }

        rtGM.AddToQueue(block, slot);
    }

    public void RemoveGapInQueue(GameObject originSlot)
    {
        int originIndex = rtGM.slotPositions.IndexOf(originSlot);

        if (originIndex >= 0 && originIndex < rtGM.Queue.Count)
        {
            rtGM.Queue.RemoveAt(originIndex);

            if (rtGM.Queue.Count != 0)
            {
                for (int i = originIndex; i < rtGM.Queue.Count; i++)
                {
                    GameObject blockToMove = rtGM.slotPositions[i + 1].transform.GetChild(0).gameObject;
                    rtGM.Queue.RemoveAt(i);
                    PlaceBlock(blockToMove, rtGM.slotPositions[i], rtGM.slotPositions[i + 1]);
                }
            }

            rtGM.slotPositions[rtGM.Queue.Count].tag = "Slot";
        }
    }

    /**private void OnTriggerEnter2D(Collider2D collision)
    {
        if (destinationSlot == null && !scrolling && !collision.CompareTag("Scroll")) destinationSlot = collision.gameObject;
    }**/

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (destinationSlot == null) destinationSlot = collision.gameObject;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (destinationSlot != null) destinationSlot = null;
    }
}
