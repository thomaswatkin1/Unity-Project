using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIControllerTitle : MonoBehaviour
{
    public void Play()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    }

    public void Tutorial()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("HowToPlay");
    }
    
    public void Exit()
    {
        Debug.Log ("QUIT!");
        Application.Quit();
    }
}
