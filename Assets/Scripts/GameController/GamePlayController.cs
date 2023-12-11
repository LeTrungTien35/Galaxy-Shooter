using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GamePlayController : MonoBehaviour
{
    public static GamePlayController instance;

    public TMP_Text textCoin;
    public TMP_Text lifeText;
    public TMP_Text stageText;
    public TMP_Text ScoreText;
    public TMP_Text LevelText;

    public GameObject pausePanel;
    public GameObject WinPanel;
    public GameObject gameOverPanel;
    public GameObject pauseButton;
    public GameObject leveCompletePanel;
    public GameObject endText;

    static int level = 1; // level nguoi choi dat duoc
    static int score = 0; // diem so
    static int lifes = 3; // mang song

    int enemyAmount; // so luong enemy

    int scoreToBonusLife = 1000; // diem thuong cho life

    static int bonusScore; // diem thuong
    static bool hasLost; // da thua


    void Awake()
    {
        MakeInstance();
      
        if (hasLost)
        {
            level = 1;
            score = 0;
            lifes = 3;
            bonusScore = 0;
            hasLost = false;
        }
        endText.SetActive(false);
        leveCompletePanel.SetActive(false);
        pausePanel.SetActive(false);
        WinPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        pauseButton.SetActive(true);
    }

    private void Start()
    {
        UpdateCoinText(score);
        UpdateLifeText(lifes);
        ShowStageText(level);
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateCoinText(score);
        bonusScore += amount;
        if(bonusScore >= scoreToBonusLife)
        {
            lifes++;
            bonusScore %= scoreToBonusLife;
        }
    }

    // CHECK GAME OVER
    public void DecreaseLifes()
    {
        lifes--;
        UpdateLifeText(lifes);
        if(lifes <= 0)
        {
            hasLost = true;
            CheckHighScoreAndHighLevel();
            StartCoroutine(GameOver());
        }
    }

    public void winCondition()
    {
        level++;
        StartCoroutine(LevelCompletePanel());        
    }

    void MakeInstance()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }    

    // Cap nhat coin text
    public void UpdateCoinText(int amount)
    {
        textCoin.text = amount.ToString();
    }

    // Cap nhat mang song
    public void UpdateLifeText(int amount)
    {
        lifeText.text = "X" + amount.ToString();
    }

    // Hien thi stage 
    public void ShowStageText(int amount)
    {
        stageText.gameObject.SetActive(true);
        stageText.text = "Stage " + amount;
        StartCoroutine(DelayShowStage());
    }

    public IEnumerator DelayShowStage()
    {
        yield return new WaitForSeconds(3f);
        stageText.gameObject.SetActive(false);
    }

    public void PauseGame()
    {
        AudioManager.instance.PlaySFX(AudioManager.instance.button);
        pausePanel.SetActive(true);
        pauseButton.SetActive(false);
        Time.timeScale = 0f;
    }
    
    public void ResumeGame()
    {
        pausePanel.SetActive(false);
        pauseButton.SetActive(true);
        Time.timeScale = 1f;
    }

    public void BackHome()
    {
        if (Time.timeScale == 0f)
        {
            Time.timeScale = 1f;
        }
        AudioManager.instance.PlayMusic(AudioManager.instance.backGround_menu, true);
        SceneManager.LoadScene("MainMenu");          
    }

    public void Restart()
    {
        if (Time.timeScale == 0f)
        {
            Time.timeScale = 1f;
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
  
    private IEnumerator GameOver()
    {
        yield return new WaitForSeconds(1.5f);
        Debug.Log("Game over");
        Time.timeScale = 0f;
        gameOverPanel.SetActive(true);
        pauseButton.SetActive(false);

        ScoreText.text = "SCORE: " + score;
        LevelText.text = "LEVEL: " + level;
    }    

    public IEnumerator LevelCompletePanel()
    {
        yield return new WaitForSeconds(1f);
        endText.SetActive(true);
        yield return new WaitForSeconds(2.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void CheckHighScoreAndHighLevel()
    {
        if(ScoreManager.instance != null)
        {
            if (score > ScoreManager.instance.GetHighScore())
            {
                Debug.Log("Diem moi la: " + score);
                ScoreManager.instance.SetHighScore(score);
            }
            if (level > ScoreManager.instance.GetHighLevel())
            {
                Debug.Log("level moi la: " + level);
                ScoreManager.instance.SetHighLevel(level);
            }
        }           
    }

}
