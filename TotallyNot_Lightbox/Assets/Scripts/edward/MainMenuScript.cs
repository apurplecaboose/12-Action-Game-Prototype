using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public void QUIT_THEGAME()
    {
        Application.Quit();
    }
    public void TO_MAIN_MENU()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void LOADLEVEL_1()
    {

    }
    public void LOADLEVEL_2()
    {

    }
    public void LOADLEVEL_3()
    {

    }
    public void LOADLEVEL_4()
    {

    }

}
