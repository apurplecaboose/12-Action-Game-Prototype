using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TileReel_Manager : MonoBehaviour
{
    //public enum BlockType
    //{
    //    Left,
    //    Right,
    //    LeftJump,
    //    RightJump
    //}
    //[SerializeField] List<BlockType> _InputSequence;

    public TileType LeftPrefab;
    public TileType RightPrefab;
    public TileType LeftJumpPrefab;
    public TileType RightJumpPrefab;
    public InvisibleFinishTile FinishTile;
    RectTransform _finishTileRectTransform;
    RectTransform _JudgementLine;
    float _ScrollSpeed = 100;
    public RectTransform TileParent;

    [SerializeField] List<gameManager.BlockTypes> _InputSequence;

    List<Image> _tileimages;
    private void Awake()
    {
        gameManager gm = FindFirstObjectByType<gameManager>();
        List<gameManager.BlockTypes> copyinputlist = new();
        copyinputlist = gm.Queue;
        _InputSequence = new();
        _InputSequence = copyinputlist;
    }
    void Start()
    {
        _tileimages = new();
        _JudgementLine = this.GetComponent<RectTransform>();

        //for (int i = 0; i < _InputSequence.Count; i++)
        //{
        //    Transform instance = null;
        //    if (_InputSequence[i] == BlockType.Left)
        //    {
        //        instance = Instantiate(LeftPrefab, TileParent.transform).transform;  
        //    }
        //    else if (_InputSequence[i] == BlockType.Right)
        //    {
        //        instance = Instantiate(RightPrefab, TileParent.transform).transform;
        //    }
        //    else if (_InputSequence[i] == BlockType.LeftJump)
        //    {
        //        instance = Instantiate(LeftJumpPrefab, TileParent.transform).transform;
        //    }
        //    else if (_InputSequence[i] == BlockType.RightJump)
        //    {
        //        instance = Instantiate(RightJumpPrefab, TileParent.transform).transform;
        //    }
        //    if (instance != null)
        //    {
        //        _tileimages.Add(instance.GetComponent<Image>());
        //    }
        //}
        for (int i = 0; i < _InputSequence.Count; i++)
        {
            Transform instance = null;
            if (_InputSequence[i] == gameManager.BlockTypes.Left)
            {
                instance = Instantiate(LeftPrefab, TileParent.transform).transform;
            }
            else if (_InputSequence[i] == gameManager.BlockTypes.Right)
            {
                instance = Instantiate(RightPrefab, TileParent.transform).transform;
            }
            else if (_InputSequence[i] == gameManager.BlockTypes.LeftJump)
            {
                instance = Instantiate(LeftJumpPrefab, TileParent.transform).transform;
            }
            else if (_InputSequence[i] == gameManager.BlockTypes.RightJump)
            {
                instance = Instantiate(RightJumpPrefab, TileParent.transform).transform;
            }
            if (instance != null)
            {
                _tileimages.Add(instance.GetComponent<Image>());
            }
        }
        _finishTileRectTransform = (RectTransform)Instantiate(FinishTile, TileParent.transform).transform; // Add the finish tile last AND CAST IT TO RECT TRANSFORM?!?!?!?!?!?
    }

    void Update()
    {
        ScrollReel();
        if (CheckFinalInvisibletile()) return;
        UpdateTileStates();
    }
    bool CheckFinalInvisibletile()
    {
        if (IsOverlapping(_JudgementLine, _finishTileRectTransform))
        {
            //disable last tile
            Image lastimage = _tileimages[_tileimages.Count - 1];
            lastimage.color = Color.gray;
            if (lastimage.transform.TryGetComponent<TileType>(out var foundcomponent))
            {
                foundcomponent.enabled = false;
            }
            _ScrollSpeed += 50 * Time.deltaTime;
            _ScrollSpeed = Mathf.Clamp(_ScrollSpeed, _ScrollSpeed, 420);
            //activate finish tile script
            _finishTileRectTransform.transform.GetComponent<InvisibleFinishTile>().enabled = true;
            print("it has reached the end of the reel");
            return true;
        }
        else return false;
    }
    void UpdateTileStates()
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
    void ScrollReel()
    {
        Vector3 pos = TileParent.position;

        // Reference resolution width
        float referenceWidth = 1920f;

        // Scale factor based on current screen width
        float resolutionScale = Screen.width / referenceWidth;

        // Optional: smooth variation using Perlin noise
        float theslightestsummerbreeze = 1 + (0.1f * Mathf.PerlinNoise1D(Time.time));

        // Apply scaled scroll speed
        pos.x -= _ScrollSpeed * resolutionScale * Time.deltaTime * theslightestsummerbreeze;

        TileParent.position = pos;
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
