using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.SocialPlatforms.Impl;

public class MenuController : MonoBehaviour
{
    public TMP_Text highScore;
    public TMP_Text highLevel;

    private void Start()
    {
        if(ScoreManager.instance != null)
        {
            highScore.text = "HIGH SCORE: " + ScoreManager.instance.GetHighScore().ToString();
            highLevel.text = "HIGH LEVEL: " + ScoreManager.instance.GetHighLevel().ToString();
        }          
    }
    public void LoadScene1()
    {
        SceneManager.LoadScene("Level 1");
    }

}
