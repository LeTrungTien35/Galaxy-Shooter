using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoving : MonoBehaviour
{
    [SerializeField]
    float enemyBulletSpawnTime = 10f;
    [SerializeField]
    int bulletDamage = -1;

    public Path pathToFollow;
    public int currentWayPointID = 0;
    public float speed = 2;
    public float reachDistance = 0.4f;
    public float rotationSpeed = 5f;

    float distance;
    public bool useBezier;

    //HEALTH
    public int health = 2;

    //SHOOTING 
    public GameObject bullet;
    public Transform spawnPoint;

    // DIEM CONG CHO PLAYER
    public int score = 0;

    // HIEU UNG NO ENEMY
    public GameObject enemyExplosionPrefab;

    public enum EnemyStates
    {
        ON_PATH,
        FLY_IN,
        IDLE,
        DIVE
    }

    public EnemyStates enemyStates;

    public int enemyID;
    public Formation formation;

    void Start()
    {
        StartCoroutine(Shoot());
    }

    // Update is called once per frame
    void Update()
    {
        switch (enemyStates)
        {
            case EnemyStates.ON_PATH:
                MoveOnThePath(pathToFollow);
                break;
            case EnemyStates.FLY_IN:
                MoveToFormation();
                break;
            case EnemyStates.IDLE:
                
                break;
            case EnemyStates.DIVE:
                MoveOnThePath(pathToFollow);
                break;
        }
    }

    void MoveToFormation()
    {
        transform.position = Vector3.MoveTowards(transform.position, formation.GetVector(enemyID), speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, formation.GetVector(enemyID)) <= 0.0001f)
        {
            transform.SetParent(formation.gameObject.transform);
            transform.eulerAngles = Vector3.zero;

            formation.enemyList.Add(new Formation.EnemyFormation(enemyID, transform.localPosition.x, transform.localPosition.y, this.gameObject));

            enemyStates = EnemyStates.IDLE;
        }
    }
    void MoveOnThePath(Path path)
    {
        if (useBezier)
        {
            distance = Vector3.Distance(path.bezierObjList[currentWayPointID], transform.position);
            transform.position = Vector3.MoveTowards(transform.position, path.bezierObjList[currentWayPointID], speed * Time.deltaTime);          
        }
        else
        {
            distance = Vector3.Distance(path.pathObjList[currentWayPointID].position, transform.position);
            transform.position = Vector3.MoveTowards(transform.position, path.pathObjList[currentWayPointID].position, speed * Time.deltaTime);
        }

        if (useBezier)
        {
            if (distance <= reachDistance)
            {
                currentWayPointID++;
            }

            if (currentWayPointID >= path.bezierObjList.Count)
            {
                currentWayPointID = 0;

                // DIVING
                if(enemyStates == EnemyStates.DIVE)
                {
                    transform.position = GameObject.Find("EnemySpawnPoint").transform.position;
                    Destroy(pathToFollow.gameObject);
                }

                enemyStates = EnemyStates.FLY_IN;
            }
        }
        else
        {
            if (distance <= reachDistance)
            {
                currentWayPointID++;
            }

            if (currentWayPointID >= path.pathObjList.Count)
            {
                currentWayPointID = 0;
                enemyStates = EnemyStates.FLY_IN;
            }
        }
    }

    public void SpawnSetup(Path path, int ID, Formation _formation)
    {
        pathToFollow = path;
        enemyID = ID;
        formation = _formation;
    }

    public void DiveSetup(Path path)
    {
        pathToFollow = path;
        transform.SetParent(transform.parent.parent);
        enemyStates = EnemyStates.DIVE;
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        if(health <= 0)
        {
            //PLAY SOUND
            AudioManager.instance.PlaySFX(AudioManager.instance.explosionSound_Enemy);
            // INSTATIATE PRATICLE
            GameObject enemyExplosion = Instantiate(enemyExplosionPrefab, transform.position, Quaternion.identity);
            Destroy(enemyExplosion, 0.4f);

            // ADD SCORE
            GamePlayController.instance.AddScore(score);

            //REPORT TO FORMATION
            for (int i = 0; i < formation.enemyList.Count; i++)
            {
                if (formation.enemyList[i].index == enemyID)
                {
                    formation.enemyList.Remove(formation.enemyList[i]);
                }
            }

            //REPORT TO SPAWN MANAGER
            SpawnManager sp = GameObject.Find("EnemySpawnPoint").GetComponent<SpawnManager>();
            sp.UpdateSpawnedEnemies(gameObject);
            //for (int i = 0; i < sp.spawnedEnemies.Count; i++)
            //{
            //    sp.spawnedEnemies.Remove(this.gameObject);
            //}

            //REPORT TO GAME PLAY CONTROLLER
            //GamePlayController.instance.ReduceEnemy();

            Destroy(gameObject);
        }    
    }
    
    void Fire()
    {
        if (bullet != null && spawnPoint != null && enemyStates == EnemyStates.IDLE)
        {
            GameObject newBullet = Instantiate(bullet, spawnPoint.position, Quaternion.identity);
            newBullet.GetComponent<BulletController>().SetDamage(bulletDamage);
        }       
    }
    IEnumerator Shoot()
    {
        yield return new WaitForSeconds(enemyBulletSpawnTime);
        Fire();
        StartCoroutine(Shoot());
    }
}
