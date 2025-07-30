using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolSlotManager : MonoBehaviour
{
    public GameObject pool1, pool2, pool3, pool4;
    TextMesh leftMesh1, rightMesh2, lJumpMesh3, rJumpMesh4;
    void Start()
    {
        leftMesh1 = pool1.transform.GetChild(0).GetComponent<TextMesh>(); leftMesh1.GetComponent<MeshRenderer>().sortingOrder = 10;
        rightMesh2 = pool2.transform.GetChild(0).GetComponent<TextMesh>(); rightMesh2.GetComponent<MeshRenderer>().sortingOrder = 10;
        lJumpMesh3 = pool3.transform.GetChild(0).GetComponent<TextMesh>(); lJumpMesh3.GetComponent<MeshRenderer>().sortingOrder = 10;
        rJumpMesh4 = pool4.transform.GetChild(0).GetComponent<TextMesh>(); rJumpMesh4.GetComponent<MeshRenderer>().sortingOrder = 10;

        UpdateText();
    }

    public void UpdateText()
    {
        leftMesh1.text = pool1.transform.childCount - 1 + "x";
        if (leftMesh1.text == "0x") leftMesh1.color = new Color(0.5f, 0.5f, 0.5f, 1f);
        else leftMesh1.color = new Color(0f, 0f, 0f, 1f);
        rightMesh2.text = pool2.transform.childCount - 1 + "x";
        if (rightMesh2.text == "0x") rightMesh2.color = new Color(0.5f, 0.5f, 0.5f, 1f);
        else rightMesh2.color = new Color(0f, 0f, 0f, 1f);
        lJumpMesh3.text = pool3.transform.childCount - 1 + "x";
        if (lJumpMesh3.text == "0x") lJumpMesh3.color = new Color(0.5f, 0.5f, 0.5f, 1f);
        else lJumpMesh3.color = new Color(0f, 0f, 0f, 1f);
        rJumpMesh4.text = pool4.transform.childCount - 1 + "x";
        if (rJumpMesh4.text == "0x") rJumpMesh4.color = new Color(0.5f, 0.5f, 0.5f, 1f);
        else rJumpMesh4.color = new Color(0f, 0f, 0f, 1f);
    }
}
