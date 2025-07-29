using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stretcher : MonoBehaviour
{
    public Transform Icon, Background, Anchor;

    public float scaleSpeed = 1f;
    public float minScaleX = 1f;
    public float maxScaleX = 5f;
    void Start()
    {
        transform.localScale = Vector3.one;
    }



    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll != 0f)
        {
            Vector3 newScale = Anchor.localScale;
            newScale.x += scroll * scaleSpeed;
            newScale.x = Mathf.Clamp(newScale.x, minScaleX, maxScaleX);
            Anchor.localScale = newScale;
        }


        Icon.position = Background.position;
    }
}
