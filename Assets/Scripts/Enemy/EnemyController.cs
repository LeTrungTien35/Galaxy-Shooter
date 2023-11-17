using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Transform []gunPoint;  // vi tri ban dan 
    public GameObject enemyBullet; // Khai bao vien dan
    public float enemyBulletSpawnTime = 1f; // Thoi gian spawn ra vien dan
    public float speedMovement = 1f; // toc do di chuyen cua enemy
    public GameObject enemyExplosionPrefab; // Hieu ung no enemy
    public HealthbarController healthbarController; // Khai bao class dieu khien healthbar
    public GameObject damageEffectPrefabs;  // Hieu ung vien dan va cham voi phi thuyen
    public GameObject coinPrefabs;  // Coin sinh ra khi ban ha enemy

    public float health = 10f;  // so luong mau cua enemy

    float healthbarSize = 1f; // kich thuoc cua thanh suc khoe
    float damage = 0f; // sat thuong tinh theo phan tram

    public AudioSource audioSource;
    public AudioClip explosionSound;
    void Start()
    {
        StartCoroutine(Shoot());
        damage = healthbarSize / health;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.down * speedMovement * Time.deltaTime);
    }

    void Fire()
    {
        foreach(Transform gun in gunPoint)
        {
            Instantiate(enemyBullet, gun.position, Quaternion.identity);
        }
    }

    IEnumerator Shoot()
    {
        yield return new WaitForSeconds(enemyBulletSpawnTime);
        Fire();
        StartCoroutine(Shoot());
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "PlayerBullet")
        {
            //Destroy(col.gameObject);
            DamageHealthbar();
            GameObject damageEffect = Instantiate(damageEffectPrefabs, col.transform.position, Quaternion.identity);
            Destroy(damageEffect, 0.1f);
            if (health <= 0)
            {
                AudioSource.PlayClipAtPoint(explosionSound, Camera.main.transform.position);
                Instantiate(coinPrefabs, transform.position, Quaternion.identity);
                Destroy(gameObject);
                GameObject enemyExplosion = Instantiate(enemyExplosionPrefab, transform.position, Quaternion.identity);
                Destroy(enemyExplosion, 0.4f);
            }    
        }
    }

    void DamageHealthbar()
    {
        if(health > 0)
        {
            health -= 1;
            healthbarSize = healthbarSize - damage;
            healthbarController.setSizeBar(healthbarSize);
        }    
    }    
}
