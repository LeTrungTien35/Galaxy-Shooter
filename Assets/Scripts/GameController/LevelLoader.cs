using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    private int currentIndex;
    void Start()
    {
        currentIndex = SceneManager.GetActiveScene().buildIndex;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NextLevel()
    {
        if (Time.timeScale == 0f)
        {
            Time.timeScale = 1f;
        }
        SceneManager.LoadScene(currentIndex + 1);
    }    
    public void Restart()
    {
        if(Time.timeScale == 0f)
        {
            Time.timeScale = 1f;
        }    
        SceneManager.LoadScene(currentIndex);
    }    
}
