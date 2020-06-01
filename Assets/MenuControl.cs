using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuControl : MonoBehaviour
{

    public GameObject menu;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }


    public void TogglePause()
    {
        if (menu.activeSelf)
        {
            Time.timeScale = 1.0f;
            menu.SetActive(false);
            Cursor.visible = false;
            AudioListener.volume = 1.0f;
        }
        else
        {
            Time.timeScale = 0.0f;
            menu.SetActive(true);
            Cursor.visible = true;
            AudioListener.volume = 0f;
        }
    }

    public void Exit()
    {
        Application.Quit();
    }
}
