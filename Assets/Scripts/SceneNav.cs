using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneNav : MonoBehaviour
{
    public GameObject uiobj1;
    public GameObject uiobj2;
    public GameObject uiobj3;

    public void sceneloader(string name)
    {
        if(name == "MainMenu")
        {
            Destroy(GameManager.instance);
        }
        SceneManager.LoadScene(name);
    }
    public void quit()
    {
        Application.Quit();
    }

    public void options()
    {
        uiobj1.SetActive(true);
        uiobj2.SetActive(false);
    }
    public void mainmenu()
    {
        uiobj1.SetActive(false);
        uiobj2.SetActive(true);
        uiobj3.SetActive(false);
    }
    public void Mode()
    {
        uiobj3.SetActive(true);
        uiobj2.SetActive(false);
    }
}
