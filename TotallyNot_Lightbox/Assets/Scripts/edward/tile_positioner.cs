using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tile_positioner : MonoBehaviour
{
    public Transform PreviousTile;
    float _localscaleX;
    void Start()
    {
       
    }

    void Update()
    {
        _localscaleX = PreviousTile.transform.localScale.x;
        float xposition = _localscaleX + PreviousTile.transform.position.x;
        this.transform.position = new Vector3(xposition, this.transform.position.y, this.transform.position.z);
    }
}
