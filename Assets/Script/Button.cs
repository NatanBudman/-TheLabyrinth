using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Button : MonoBehaviour
{
 

    public void ChangeScene(string SceneName)
    {
        SceneManager.LoadScene($"Scenes/{SceneName}");


    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
