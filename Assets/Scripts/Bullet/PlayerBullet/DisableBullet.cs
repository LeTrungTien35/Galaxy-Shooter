using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableBullet : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("DestroyZone") || col.gameObject.CompareTag("EnemyShip"))
        {
            gameObject.SetActive(false);
        }
        else 
        {
            return;
        }  
    }
}
