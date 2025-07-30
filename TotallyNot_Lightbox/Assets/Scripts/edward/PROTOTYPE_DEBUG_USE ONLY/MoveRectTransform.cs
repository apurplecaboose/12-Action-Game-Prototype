using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRectTransform : MonoBehaviour
{
    RectTransform _t;
    void Start()
    {
        _t = GetComponent<RectTransform>();
    }
    void Update()
    {
        Vector3 pos = _t.position;

        if (Input.GetMouseButton(0)) // Left mouse held
        {
            pos.x -= 100f * Time.deltaTime;
        }

        if (Input.GetMouseButton(1)) // Right mouse held
        {
            pos.x += 600f * Time.deltaTime;
        }

        _t.position = pos;
    }
}