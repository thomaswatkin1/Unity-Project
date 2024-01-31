using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UIController : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasgroup;

    Player player;
    TextMeshProUGUI distanceText;   
    GameObject results;
    TextMeshProUGUI finalDistanceText;
    private bool fadeIn = false;


    private void Awake()
    {
        player = GameObject.Find("Player")?.GetComponent<Player>();
        distanceText = GameObject.Find("DistanceText")?.GetComponent<TextMeshProUGUI>();         
        finalDistanceText = GameObject.Find("FinalDistance")?.GetComponent<TextMeshProUGUI>();
        results = GameObject.Find("Results");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        UpdateDistanceText();
        CheckPlayerStatus();

        if (fadeIn)
        {
            if(canvasgroup.alpha < 1)
            {
                canvasgroup.alpha += Time.fixedDeltaTime * 1f;
                if(canvasgroup.alpha >= 1)
                {
                    fadeIn = false;
                }
            }
        }
    }

    private void Switch()
    {
        
    }

    private void UpdateDistanceText()
    {
        if (distanceText != null)
        {
            int distance = Mathf.FloorToInt(player.distance);
            distanceText.text = distance + " m";
        }

    }

    private void CheckPlayerStatus()
    {
        if (player != null)
        {
            if (player.isDead)
            {
                results.SetActive(true);
                FadeIn();
                UpdateFinalDistanceText();
            }
            else
            {
                results.SetActive(false);
            }
        }
    }

    private void UpdateFinalDistanceText()
    {
        int distance = Mathf.FloorToInt(player.distance);
        finalDistanceText.text = distance + " m";
    }

    public void FadeIn()
    {
        fadeIn = true;
    }

    public void Quit()
    {
        SceneManager.LoadScene("Menu");
    }

    public void Retry()
    {
        SceneManager.LoadScene("Game");
    }
}
