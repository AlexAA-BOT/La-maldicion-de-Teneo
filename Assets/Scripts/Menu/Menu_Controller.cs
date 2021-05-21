using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu_Controller : MonoBehaviour
{

    public void NewGame()
    {
        SceneManager.LoadScene("1-Room-1");
    }

    public void Options()
    {

    }

    public void ExitGame()
    {
        Application.Quit();
    }

}
