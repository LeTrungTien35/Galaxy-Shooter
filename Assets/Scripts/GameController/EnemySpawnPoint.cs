using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnPoint : MonoBehaviour
{
    public GameObject []enemy;
    public float spawnTime = 1f;
    public int enemySpawnCount = 10;
    private bool lastEnemySpawn = false;
    void Start()
    {
        StartCoroutine(EnemySpawner());
    }

    
    void Update()
    {
        if(lastEnemySpawn == true && FindObjectOfType<EnemyController>() == null)
        {
            StartCoroutine(GamePlayController.instance.LevelCompletePanel());
        }    
    }

    IEnumerator EnemySpawner()
    {
        for(int i = 0; i < enemySpawnCount; i++)
        {
            yield return new WaitForSeconds(spawnTime);
            SpawnEnemy();
        }

        lastEnemySpawn = true;
    }

    void SpawnEnemy()
    {
        int randomValue = Random.Range(0, enemy.Length);
        float randomXPos = Random.Range(-2.3f, 2.3f);
        Instantiate(enemy[randomValue], new Vector2(randomXPos, transform.position.y), Quaternion.identity);
    }    
}
