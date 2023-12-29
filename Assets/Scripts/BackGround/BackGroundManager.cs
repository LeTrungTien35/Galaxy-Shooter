using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundManager : MonoBehaviour
{
    public GameObject[] backgrounds; 

    private void Start()
    {
        SwitchBackground();
    }

    void SwitchBackground()
    {
        // Ch?n m?t background ng?u nhi�n t? m?ng
        int randomIndex = Random.Range(0, backgrounds.Length);


        for (int i = 0; i < backgrounds.Length; i++)
        {
            backgrounds[i].SetActive(false);
        }

        // Hi?n th? background ?� ch?n ng?u nhi�n
        backgrounds[randomIndex].SetActive(true);
    }
}
