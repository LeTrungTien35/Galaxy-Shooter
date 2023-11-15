using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerHealthbar : MonoBehaviour
{
    public Image healthbar;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetAmount(float amount)
    {
        healthbar.fillAmount = amount;
    }    
}
