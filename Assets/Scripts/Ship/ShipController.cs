using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    public GameObject shipExplosionPrefab;
    public PlayerHealthbar healthbar;
    public GameObject damageEffectPrefabs;
    public float health = 20f;
    float barFillAmount = 1f;
    float damage = 0;
    public int coin;

    public AudioSource audioSource;
    public AudioClip damageSound;
    public AudioClip explosionSound;
    void Start()
    {
        damage = barFillAmount / health;
    }


    void Update()
    {
             
    }
   

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "EnemyBullet")
        {
            DamagePlayerHealthbar();
            Destroy(col.gameObject);
            GameObject damageEffect = Instantiate(damageEffectPrefabs, col.transform.position, Quaternion.identity);
            Destroy(damageEffect, 0.1f);
            if(health <= 0)
            {
                AudioSource.PlayClipAtPoint(explosionSound, Camera.main.transform.position);
                Destroy(gameObject);
                GameObject explosion = Instantiate(shipExplosionPrefab, transform.position, Quaternion.identity);
                Destroy(explosion, 2f);
                GamePlayController.instance.StartCoroutineGameOver();
            }              
        }  
        
        if(col.gameObject.tag == "Coin")
        {
            coin++;
            Destroy(col.gameObject);
            if(GamePlayController.instance != null)
            {
                GamePlayController.instance.SetCoin(coin);
            }    
        }    

    }
    
    void DamagePlayerHealthbar()
    {
        if(health > 0)
        {
            health -= 1;
            barFillAmount = barFillAmount - damage;
            healthbar.SetAmount(barFillAmount);
        }    
    }    
}
