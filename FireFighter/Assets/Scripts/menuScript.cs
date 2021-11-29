using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menuScript : MonoBehaviour
{
    public void btnStart()
    {
        SceneManager.LoadScene("Game");
    }

    public void btnMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void btnExit()
    {
        Application.Quit();
    }

}
