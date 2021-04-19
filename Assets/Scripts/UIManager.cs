using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static bool gameIsPaused = false;

    public GameObject pauesmenuUI;
    public GameObject infoUI;

    public Text timecount;
    private float min;
    private float SecCount;
    private int sec;

    private void Start()
    {
        Time.timeScale = 0f;
    }
    // Update is called once per frame
    void Update()
    {
        //set time
        SecCount += Time.deltaTime;
        sec = (int)SecCount;
        timecount.text = "0" + min + ":" + sec;
        if (sec == 60)
        {
            min = 1;
            SecCount = 0;
        }
        //pause
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(gameIsPaused)
            {
                Resume();
                infoUI.SetActive(false);
            }
            else
            {
                Paused();
            }
        }
        if(Input.GetKeyDown(KeyCode.Return))
        {
            Time.timeScale = 1f;
            infoUI.SetActive(false);
        }
    }
    public void Resume()
    {
        pauesmenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }

    public void Paused()
    {
        pauesmenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
    }
}
