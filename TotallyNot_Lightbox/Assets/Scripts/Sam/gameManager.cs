using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManager : MonoBehaviour
{
    public enum blockTypes { Left, Right, LeftJump, RightJump }; //The different types of blocks available

    public List<blockTypes> queue = new List <blockTypes>(); //List of actions as defined by player in phase 1

    void Start()
    {
        //GameObjects in Pool are displayed to the player.
        //Player can then select objects from Pool or Queue. Click and Dragging moves said object with the cursor.
        //If the object comes from pool, and is released in the queue, it is added into the position where the cursor is.
        //Similarly, if the object comes from queue, and is released in pool, it is added into position where the cursor is.
        //Make use of Mouse object, with triggers and colliders to detect when aboce certain spaces in the Pool or Queue.

    }

    void Update()
    {
        //queue.Add(blockTypes.Left); When object with Left type is dragged into queue.
    }

    void PlaceIntoQueue (blockTypes block, int index) //This places the block into the index, moving all later blocks backwards 1 index.
    {

    }   
    
    void PushDownQueue () //This ensures there are no empty slots in the queue
    {
         
    }
}
