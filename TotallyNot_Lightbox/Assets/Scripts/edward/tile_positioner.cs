using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tile_positioner : MonoBehaviour
{
    public List<GameObject> Tiles;
    GameObject _StartingTile;
    void Start()
    {
        _StartingTile = Tiles[0];
    }
    private void Update()
    {
    }
}
