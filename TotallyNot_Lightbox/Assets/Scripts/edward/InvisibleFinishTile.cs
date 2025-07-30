using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InvisibleFinishTile : MonoBehaviour
{
    void Start()
    {
        print("finishtile has been reached ");
        Invoke("LoadLoseScene", 4.20f);
    }
    void LoadLoseScene()
    {
        SceneManager.LoadScene("LoseScene");
    }
}
