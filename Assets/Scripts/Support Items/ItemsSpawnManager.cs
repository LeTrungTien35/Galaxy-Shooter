using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsSpawnManager : MonoBehaviour
{
    public GameObject IconRocketPrefab;

    public GameObject livePrefab;
    public GameObject rocketPrefab;

    public int numberOfRockets = 10;

    public float spawnTime = 0;

    public Transform spawnPointTop;
    public Transform spawnPointDown;
    void Start()
    {
        StartCoroutine(LiveSpawner());
        StartCoroutine(RoketsSpawner());
        //StartCoroutine(FireRockets());
    }

    
    void Update()
    {
        
    }


    IEnumerator LiveSpawner()
    {
        yield return new WaitForSeconds(Random.Range(spawnTime, spawnTime + 5f));
        SpawnLiveItems();
        StartCoroutine(LiveSpawner());
    }

    IEnumerator RoketsSpawner()
    {
        yield return new WaitForSeconds(Random.Range(spawnTime, spawnTime + 5f));
        SpawnRoletsItems();
        StartCoroutine(RoketsSpawner());
    }

    void SpawnLiveItems()
    {
        float randomXPos = Random.Range(-2f, 2f);
        Instantiate(livePrefab, new Vector2(randomXPos, spawnPointTop.position.y), Quaternion.identity);
    }

    void SpawnRoletsItems()
    {
        float randomXPos = Random.Range(-2f, 2f);
        Instantiate(IconRocketPrefab, new Vector2(randomXPos, spawnPointTop.position.y), Quaternion.identity);
        //Debug.Log("Da spawn ten lua");
    }


    public void PickupRocketItem()
    {
        StartCoroutine(FireRockets());
    }

    IEnumerator FireRockets()
    {
        for (int i = 0; i < numberOfRockets; i++)
        {
            float randomXPos = Random.Range(-2f, 2f);
            Instantiate(rocketPrefab, new Vector2(randomXPos, spawnPointDown.position.y), Quaternion.identity);
            if (AudioManager.instance != null)
            {
                AudioManager.instance.PlaySFX(AudioManager.instance.shootRocket);
            }
            Debug.Log("Da ban rocket");
            yield return new WaitForSeconds(0.5f);
        }
    }
}
