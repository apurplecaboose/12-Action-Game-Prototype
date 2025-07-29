using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

public class Stretcher : MonoBehaviour
{
    public Transform Icon, Background, Anchor;

    public float scaleSpeed = 1f;
    public float minScaleX = 1f;
    public float maxScaleX = 5f;
    public float Current_X_Scale;
    public float NextTile_X_Position;
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
            newScale.x = Mathf.Round(newScale.x * 10f) * 0.1f;
            Current_X_Scale = newScale.x;
            Anchor.localScale = newScale;
        }


        Icon.position = Background.position;
    }
}
