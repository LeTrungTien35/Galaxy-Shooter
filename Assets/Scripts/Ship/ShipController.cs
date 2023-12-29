using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    public GameObject shipExplosionPrefab;
    public GameObject damageEffectPrefabs;
    public int coin;

    [SerializeField]
    int health = 3;

    // RESET
    Vector3 initPosition;
    void Start()
    {
        initPosition = transform.position;
    }


    void Update()
    {
             
    }
   

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "EnemyShip")
        {
            TakeDamage(-10);
            GameObject damageEffect = Instantiate(damageEffectPrefabs, col.transform.position, Quaternion.identity);
            Destroy(damageEffect, 0.1f);          
        }

        if(col.tag == "Live")
        {
            Destroy(col.gameObject);
            GamePlayController.instance.AddLives();
            if (AudioManager.instance != null)
            {
                AudioManager.instance.PlaySFX(AudioManager.instance.pickupItems);
            }
            Debug.Log("Them mang moi");
        }

        if (col.tag == "ItemRocket")
        {
            Destroy(col.gameObject);
            ItemsSpawnManager items = GameObject.Find("ItemsSpawnManager").GetComponent<ItemsSpawnManager>();
            items.PickupRocketItem();
            if (AudioManager.instance != null)
            {
                AudioManager.instance.PlaySFX(AudioManager.instance.pickupItems);
            }
            Debug.Log("Nhat rocket");
        }
    }
    
   

    public void TakeDamage(int amount)
    {
        health += amount;
        
        if (health <= 0)
        {
            //PLAY SOUND
            if(AudioManager.instance != null)
            {
                AudioManager.instance.PlaySFX(AudioManager.instance.explosionSound_Player);
            }    
           
            //INSTANTIATE EXPLOSION
            GameObject explosion = Instantiate(shipExplosionPrefab, transform.position, Quaternion.identity);
            Destroy(explosion, 2f);
            
            //DAT LAI VI TRI KHI DIE
            transform.position = initPosition;
            
            StartCoroutine(Rest());

        }
    }

    IEnumerator Rest()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        GamePlayController.instance.DecreaseLifes();
        Shooting shootingComponent = GetComponent<Shooting>();
        shootingComponent.CanShoot = false;     
        
        yield return new WaitForSeconds(1.8f);

        shootingComponent.CanShoot = true;
        GetComponent<BoxCollider2D>().enabled = true;
        health = 3;
    }    
}
