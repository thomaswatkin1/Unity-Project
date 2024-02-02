using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIControllerTitle : MonoBehaviour
{
    public Toggle dayNightToggle;

    private void Start()
    {
        if (PlayerPrefs.HasKey("NightModeToggleState"))
        {
            bool savedToggleState = PlayerPrefs.GetInt("NightModeToggleState") == 1;
            dayNightToggle.isOn = savedToggleState;
        }
    }

    public void Play()
    {
        if (dayNightToggle != null)
        {
            PlayerPrefs.SetInt("NightModeToggleState", dayNightToggle.isOn ? 1 : 0);
            PlayerPrefs.Save();

            string sceneToLoad = dayNightToggle.isOn ? "Night" : "Day";
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneToLoad);
        }
    }

    public void Tutorial()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("HowToPlay");
    }

    public void Exit()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }
}
