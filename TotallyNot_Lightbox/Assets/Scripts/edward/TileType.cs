using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileType : MonoBehaviour
{
    Image _image;

    //prob some enum to tell which type of tile it is
    //public float TileWidth; // view only
    public bool Resizable;
    float _resizeSpeed = 100f;
    Vector2Int _ResizeBounds = new Vector2Int(69, 420);
    void Start()
    {
        _image = this.GetComponent<Image>();
        //TileWidth = _image.rectTransform.rect.width;
        //set horizontal velocity to 0
        //rb2D.velocity = new Vector2(0, rb2D.velocity.y);
        //rb.velocity = new Vector3(0, rb.velocity.y, rb.velocity.z);
        //add jump force here and maybe and trigger a bool for early return for everything else
    }
    void Update()
    {
        if (Resizable)
        {
            ScrollChangeScale();
        }
    }
    void FixedUpdate()
    {
        print("Doing something forceful");
        //add forces to player since script is active
    }
    void ScrollChangeScale()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll != 0f)
        {
            float newScale = _image.rectTransform.rect.width;
            newScale += scroll * _resizeSpeed;
            newScale = Mathf.Clamp(newScale, _ResizeBounds.x, _ResizeBounds.y);
            _image.rectTransform.sizeDelta = new Vector2(newScale, 100);
        }
    }
}
