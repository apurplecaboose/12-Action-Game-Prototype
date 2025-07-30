using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileType : MonoBehaviour
{
    public enum TileTypeEnum
    {
        Left,
        Right,
        LeftJump,
        RightJump,
        NO_ACTION
    }
    

    Image _image;
    Rigidbody2D _rb;
    //GroundCheck _gc;
    [Header("Parameters")]
    public TileTypeEnum _type;
    public bool Resizable;
    float _resizeSpeed = 100f;
    Vector2Int _ResizeBounds = new Vector2Int(69, 420);
    float _JumpForce = 7.5f;
    float _MoveForce = 7;
    float _TopSpeed = 12f;
    void Start()
    {
        _image = this.GetComponent<Image>();
        GameObject playergameobj = GameObject.FindGameObjectWithTag("Player");
        _rb = playergameobj.GetComponent<Rigidbody2D>();
        //_gc = playergameobj.GetComponent<GroundCheck>();
        _rb.velocity = new Vector2(0, _rb.velocity.y);
        AddVerticalJumpImpulse();
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
        AddHorizontalForceToPlayer();
    }
    void AddHorizontalForceToPlayer()
    {
        if (Mathf.Abs(_rb.velocity.x) > _TopSpeed)
        {
            //dont add force already reached top speed
            return;
        }
        if (_type == TileTypeEnum.Left)
        {
            _rb.AddForce(new Vector2(-_MoveForce, 0));
        }
        else if (_type == TileTypeEnum.LeftJump)
        {
            _rb.AddForce(new Vector2(-_MoveForce, 0));
        }
        else if (_type == TileTypeEnum.Right)
        {
            _rb.AddForce(new Vector2(_MoveForce, 0));
        }
        else if (_type == TileTypeEnum.RightJump)
        {
            _rb.AddForce(new Vector2(_MoveForce, 0));
        }
    }
    void AddVerticalJumpImpulse()
    {
        if (_type == TileTypeEnum.LeftJump || _type == TileTypeEnum.RightJump)
        {
            _rb.AddForce(new Vector2(0, _JumpForce), ForceMode2D.Impulse);
            //add jump force here and maybe and trigger a bool for early return for everything else
        }
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
