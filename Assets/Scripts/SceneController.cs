using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{

    public void SetScene(string sceneId)
    {
        SceneManager.LoadScene(sceneId);
    }

    public void EndGame()
    {
        Application.Quit();
    }
}
