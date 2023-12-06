using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("Intervals")]
    public float enemySpawnInterval; //interval between ship spaws
    public float waveSpawnInterval; //interval between waes

    int currentWave;

    int flyID = 0;
    int waspID = 0;
    int bossID;

    [Header("Prefab")]
    public GameObject flyPrefab;
    public GameObject waspPrefab;
    public GameObject bossPrefab;

    [Header("Formation")]
    public Formation flyFormation; // for all small ships
    public Formation waspFormation;
    public Formation bossFormation;
    [Serializable]
    public class Wave
    {
        public int flyAmount;
        public int waspAmount;
        public int bossAmount;

        public GameObject[] pathPrefabs;

    }
    [Header("Waves")]
    public List<Wave> waveList = new List<Wave>();

    List<Path> activePathList = new List<Path>();

    [HideInInspector]
    public List<GameObject> spawnedEnemies = new List<GameObject>();

    bool spawnComplete;

    void Start()
    {
        Invoke("StartSpawn", 3f);
    }

    IEnumerator SpawnWaves()
    {

        while (currentWave < waveList.Count)
        {
            if(currentWave == waveList.Count - 1)
            {
                spawnComplete = true;
            }

            for (int i = 0; i < waveList[currentWave].pathPrefabs.Length; i++)
            {
                GameObject newPathObj = Instantiate(waveList[currentWave].pathPrefabs[i], transform.position, Quaternion.identity);
                Path newPath = newPathObj.GetComponent<Path>();
                activePathList.Add(newPath);
            }

            //FLYS FIRST
            for (int i = 0; i < waveList[currentWave].flyAmount; i++)
            {
                GameObject newFly = Instantiate(flyPrefab, transform.position, Quaternion.identity) as GameObject;
                EnemyMoving enemyMoving = newFly.GetComponent<EnemyMoving>();

                enemyMoving.SpawnSetup(activePathList[PathPingPong()], flyID, flyFormation);
                flyID++;

                spawnedEnemies.Add(newFly);

                // WAIT FOR SPAWN INTERVAL
                yield return new WaitForSeconds(enemySpawnInterval);
            }

            // WASP
            for (int i = 0; i < waveList[currentWave].waspAmount; i++)
            {
                GameObject newWasp = Instantiate(waspPrefab, transform.position, Quaternion.identity) as GameObject;
                EnemyMoving waspMoving = newWasp.GetComponent<EnemyMoving>();

                waspMoving.SpawnSetup(activePathList[PathPingPong()], waspID, waspFormation);
                waspID++;

                spawnedEnemies.Add(newWasp);

                yield return new WaitForSeconds(enemySpawnInterval);
            }

            // BOSS
            for (int i = 0; i < waveList[currentWave].bossAmount; i++)
            {
                GameObject newBoss = Instantiate(bossPrefab, transform.position, Quaternion.identity) as GameObject;
                EnemyMoving bossMoving = newBoss.GetComponent<EnemyMoving>();

                bossMoving.SpawnSetup(activePathList[PathPingPong()], bossID, bossFormation);
                bossID++;

                spawnedEnemies.Add(newBoss);


                yield return new WaitForSeconds(enemySpawnInterval);
            }

            yield return new WaitForSeconds(waveSpawnInterval);
            currentWave++;

            foreach (Path p in activePathList)
            {
                Destroy(p.gameObject);
            }

            activePathList.Clear();
        }

       
        Invoke("CheckEnemyState", 1f);
    }

    void CheckEnemyState()
    {
        bool inFormation  = false;
        for (int i = spawnedEnemies.Count - 1; i >= 0; i--)
        {
            if (spawnedEnemies[i].GetComponent<EnemyMoving>().enemyStates != EnemyMoving.EnemyStates.IDLE)
            {
                inFormation = true;
                Invoke("CheckEnemyState", 1f);
                break;
            }
        }
        inFormation = true;

        if(inFormation)
        {
            StartCoroutine(flyFormation.ActivateSpread());
            StartCoroutine(waspFormation.ActivateSpread());
            StartCoroutine(bossFormation.ActivateSpread());
            CancelInvoke("CheckEnemyState");
        }
    }

    void StartSpawn()
    {
        StartCoroutine(SpawnWaves());
        CancelInvoke("StartSpawn");
    }

    int PathPingPong()
    {
        return (flyID + bossID + waspID) % activePathList.Count;
    }


    void OnValidate()
    {
        int curFlyAmount = 0;
        for (int i = 0; i < waveList.Count; i++)
        {
            curFlyAmount += waveList[i].flyAmount;
        }


        if (curFlyAmount > 20)
        {
            Debug.LogError("Vuot qua so luong fly la 20");
        }
        else
        {
            Debug.Log("tong so luong fly: " + curFlyAmount);
        }
    }

    void ReportToGameManage()
    {
        if(spawnedEnemies.Count == 0 && spawnComplete)
        {
            GamePlayController.instance.winCondition();
        }
    }

    public void UpdateSpawnedEnemies(GameObject enemy)
    {
        spawnedEnemies.Remove(enemy);
        ReportToGameManage();
    }
}
