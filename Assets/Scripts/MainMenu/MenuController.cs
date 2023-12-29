using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public TMP_Text highScore;
    public TMP_Text highLevel;
    public Button OnOffMusic;
    public GameObject settingPanel;
    public GameObject instructionPanel;

    [SerializeField] Image musicOnIcon;
    [SerializeField] Image musicOffIcon;
    [SerializeField] Image sfxOnIcon;
    [SerializeField] Image sfxOffIcon;
    private void Start()
    {
        if(ScoreManager.instance != null)
        {
            highScore.text = "HIGH SCORE: " + ScoreManager.instance.GetHighScore().ToString();
            highLevel.text = "HIGH LEVEL: " + ScoreManager.instance.GetHighLevel().ToString();
        }

        settingPanel.SetActive(false);
        instructionPanel.SetActive(false);

        musicOffIcon.gameObject.SetActive(AudioManager.instance.musicSource.mute);
        musicOnIcon.gameObject.SetActive(!AudioManager.instance.musicSource.mute);

        sfxOffIcon.gameObject.SetActive(AudioManager.instance.sfxSource.mute);
        sfxOnIcon.gameObject.SetActive(!AudioManager.instance.sfxSource.mute);
    }
    public void LoadScene1()
    {
        AudioManager.instance.PlaySFX(AudioManager.instance.button);
        AudioManager.instance.PlayMusic(AudioManager.instance.backGround_level1, true);
        SceneManager.LoadScene("Level 1");
    }

    public void BtnOnOffMusic()
    {
        AudioManager.instance.OnOffMusic();
        if(AudioManager.instance.musicSourceMuted)
        {
            musicOffIcon.gameObject.SetActive(true);
            musicOnIcon.gameObject.SetActive(false);
        }    
        else
        {
            musicOffIcon.gameObject.SetActive(false);
            musicOnIcon.gameObject.SetActive(true);
        }    

    }

    public void BtnOnOffSFX()
    {
        AudioManager.instance.OnOffSFX();
        if (AudioManager.instance.sfxSourceMuted)
        {
            sfxOffIcon.gameObject.SetActive(true);
            sfxOnIcon.gameObject.SetActive(false);
        }
        else
        {
            sfxOffIcon.gameObject.SetActive(false);
            sfxOnIcon.gameObject.SetActive(true);
        }

    }

    public void OnOffSettingPanel()
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlaySFX(AudioManager.instance.button);
        }
        if (settingPanel.activeSelf)
        {
            settingPanel.SetActive(false);
        }    
        else
        {
            settingPanel.SetActive(true);
        }    
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void OnOffInstructionPanel()
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlaySFX(AudioManager.instance.button);
        }
        if (instructionPanel.activeSelf)
        {
            instructionPanel.SetActive(false);
        }
        else
        {
            instructionPanel.SetActive(true);
        }
    }
}
