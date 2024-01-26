using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIControllerTitle : MonoBehaviour
{
    public void Play()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
    }

    public void Tutorial()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("HowToPlay");
    }

    public void Quit()
    {
        Debug.Log ("QUIT!");
        Application.Quit();
    }
}
