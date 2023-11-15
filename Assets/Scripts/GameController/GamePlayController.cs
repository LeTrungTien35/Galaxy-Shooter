using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GamePlayController : MonoBehaviour
{
    public static GamePlayController instance;
    public TMP_Text textCoin;
    public GameObject pausePanel;
    public GameObject WinPanel;
    public GameObject gameOverPanel;
    public GameObject pauseButton;
    public GameObject leveCompletePanel;
    public GameObject endText;
    void Awake()
    {
        MakeInstance();

        endText.SetActive(false);
        leveCompletePanel.SetActive(false);
        pausePanel.SetActive(false);
        WinPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        pauseButton.SetActive(true);
    }

    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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

    public void SetCoin(int coin)
    {
        textCoin.text = coin.ToString();
    }

    public void PauseGame()
    {
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
        SceneManager.LoadScene("MainMenu");          
    }    

    public void StartCoroutineGameOver()
    {
        StartCoroutine(GameOver());
    }    
    private IEnumerator GameOver()
    {
        yield return new WaitForSeconds(1.5f);
        Debug.Log("Game over");
        Time.timeScale = 0f;
        gameOverPanel.SetActive(true);
        pauseButton.SetActive(false);      
    }    

    public IEnumerator LevelCompletePanel()
    {
        yield return new WaitForSeconds(1f);
        endText.SetActive(true);
        yield return new WaitForSeconds(2f);
        Time.timeScale = 0f;
        leveCompletePanel.SetActive(true);
        pauseButton.SetActive(false);
    }

}
