using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManager : MonoBehaviour
{
    public enum BlockTypes { Left, Right, LeftJump, RightJump }; //The different types of blocks available

    public List<BlockTypes> Queue = new List <BlockTypes>(); //List of 12 actions as defined by player in phase 1



    public List<GameObject> slotPositions = new List<GameObject>();

    public static gameManager _instance;
    private void Awake()
    {
        if(_instance == null )
        {
            DontDestroyOnLoad(gameObject);
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
       
    }

    public void AddToQueue (GameObject block, GameObject slotPosition) //This places the held block into the queue list at an index defined by slotPosition.
    {
        if (!Enum.TryParse(block.tag, out BlockTypes type))
        {
            print("Tag does not match blockTypes enum: " + block.tag); //Ensure the block has a tag and it matches blockTypes tag
            return;
        }

        int index = slotPositions.IndexOf(slotPosition);
        if (index > Queue.Count) index = Queue.Count;

        Queue.Insert(index, type);
    }
}

