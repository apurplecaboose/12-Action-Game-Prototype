using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManager : MonoBehaviour
{
    public enum blockTypes { Left, Right, LeftJump, RightJump }; //The different types of blocks available

    public List<blockTypes> queue = new List <blockTypes>(); //List of 12 actions as defined by player in phase 1
    public List<GameObject> slotPositions = new List<GameObject>();

    void Start()
    {

    }

    void Update()
    {

    }
    public class Block
    {

    }

    public void AddToQueue (GameObject block, GameObject slotPosition) //This places the held block into the queue list at an index defined by slotPosition.
    {
        if (!Enum.TryParse(block.tag, out blockTypes type))
        {
            print("Tag does not match blockTypes enum: " + block.tag); //Ensure the block has a tag and it matches blockTypes tag
            return;
        }

        int index = slotPositions.IndexOf(slotPosition);
        if (index > queue.Count) index = queue.Count;

        queue.Insert(index, type);
    }
}

