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

    int scoreToBonusLife = 1000; // diem thuong cho life

    static int bonusScore; // diem thuong
    static bool hasLost; // da thua

    public int hehe = level;

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

    /// <summary>
    /// Cong Diem 
    /// </summary>
    /// <param name="amount"></param>
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

    /// <summary>
    /// CHECK GAME OVER
    /// </summary>
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

    /// <summary>
    /// Kiem Tra Win Game
    /// </summary>
    public void winCondition()
    {
        level++;
        StartCoroutine(LevelCompletePanel());        
    }

    /// <summary>
    /// Cap nhat coin text
    /// </summary>
    /// <param name="amount"></param>
    public void UpdateCoinText(int amount)
    {
        textCoin.text = amount.ToString();
    }

    /// <summary>
    /// Cap nhat mang song
    /// </summary>
    /// <param name="amount"></param>
    public void UpdateLifeText(int amount)
    {
        lifeText.text = "X" + amount.ToString();
    }

    /// <summary>
    /// Thong bao Stage hien tai
    /// </summary>
    /// <param name="amount"></param>
    public void ShowStageText(int amount)
    {
        stageText.gameObject.SetActive(true);
        stageText.text = "Stage " + amount;
        StartCoroutine(DelayShowStage());
    }
  

    /// <summary>
    /// Cap nhat diem cao
    /// </summary>
    void CheckHighScoreAndHighLevel()
    {
        if (ScoreManager.instance != null)
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

    /// <summary>
    /// Button Pause Game
    /// </summary>
    public void PauseGame()
    {
        AudioManager.instance.PlaySFX(AudioManager.instance.button);
        pausePanel.SetActive(true);
        pauseButton.SetActive(false);
        Time.timeScale = 0f;
    }
    
    /// <summary>
    /// Button Resume Game
    /// </summary>
    public void ResumeGame()
    {
        pausePanel.SetActive(false);
        pauseButton.SetActive(true);
        Time.timeScale = 1f;
    }

    /// <summary>
    /// Button quay lai main menu
    /// </summary>
    public void BackHome()
    {
        if (Time.timeScale == 0f)
        {
            Time.timeScale = 1f;
        }
        AudioManager.instance.PlayMusic(AudioManager.instance.backGround_menu, true);
        SceneManager.LoadScene("MainMenu");          
    }

    /// <summary>
    /// Restart scene
    /// </summary>
    public void Restart()
    {
        if (Time.timeScale == 0f)
        {
            Time.timeScale = 1f;
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    /// <summary>
    /// IEnumerator hien thi Panel Game Over
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// IEnumerator hoan thanh Level
    /// </summary>
    /// <returns></returns>
    public IEnumerator LevelCompletePanel()
    {
        yield return new WaitForSeconds(1f);
        endText.SetActive(true);
        yield return new WaitForSeconds(2.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// Delay Show giai doan
    /// </summary>
    /// <returns></returns>
    public IEnumerator DelayShowStage()
    {
        yield return new WaitForSeconds(3f);
        stageText.gameObject.SetActive(false);
    }
  
}
