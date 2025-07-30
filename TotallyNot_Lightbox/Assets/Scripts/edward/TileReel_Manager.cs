using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TileReel_Manager : MonoBehaviour
{
    public enum BlockType
    {
        Left,
        Right,
        LeftJump,
        RightJump
    }

    public TileType LeftPrefab;
    public TileType RightPrefab;
    public TileType LeftJumpPrefab;
    public TileType RightJumpPrefab;

    RectTransform _JudgementLine;
    public RectTransform TileParent;

    [SerializeField] List<BlockType> _InputSequence;
    List<Image> _tileimages;
    private void Awake()
    {
        //fetch and copy _inputsequence;
    }
    void Start()
    {
        _tileimages = new();
        _JudgementLine = this.GetComponent<RectTransform>();

        for (int i = 0; i < _InputSequence.Count; i++)
        {
            Transform instance = null;
            if (_InputSequence[i] == BlockType.Left)
            {
                instance = Instantiate(LeftPrefab, TileParent.transform).transform;  
            }
            else if (_InputSequence[i] == BlockType.Right)
            {
                instance = Instantiate(RightPrefab, TileParent.transform).transform;
            }
            else if (_InputSequence[i] == BlockType.LeftJump)
            {
                instance = Instantiate(LeftJumpPrefab, TileParent.transform).transform;
            }
            else if (_InputSequence[i] == BlockType.RightJump)
            {
                instance = Instantiate(RightJumpPrefab, TileParent.transform).transform;
            }
            if (instance != null)
            {
                _tileimages.Add(instance.GetComponent<Image>());
            }
        }
    }

    void Update()
    {
        foreach (var i in _tileimages)
        {
            if (IsOverlapping(_JudgementLine, i.rectTransform))
            {
                i.color = Color.green;
                if (i.transform.TryGetComponent<TileType>(out var foundcomponent))
                {
                    foundcomponent.enabled = true;
                }
            }
            else
            {
                i.color = Color.grey;
                if (i.transform.TryGetComponent<TileType>(out var foundcomponent))
                {
                    foundcomponent.enabled = false;
                }
            }
        }
    }
    bool IsOverlapping(RectTransform a, RectTransform b)
    {
        Rect rectA = GetWorldRect(a);
        Rect rectB = GetWorldRect(b);
        return rectA.Overlaps(rectB);
        #region subfunc
        Rect GetWorldRect(RectTransform rt)
        {
            Vector3[] corners = new Vector3[4];
            rt.GetWorldCorners(corners);
            float width = corners[2].x - corners[0].x;
            float height = corners[2].y - corners[0].y;
            return new Rect(corners[0].x, corners[0].y, width, height);
        }
        #endregion
    }
}
