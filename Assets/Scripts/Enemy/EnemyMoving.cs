using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoving : MonoBehaviour
{
    [SerializeField]
    float enemyBulletSpawnTime = 10f; // THOI GIAN BAN DAN
    [SerializeField]
    int bulletDamage = -1; // DAMAGE CUA BULLET

    public Path pathToFollow; // PATH 
    public int currentWayPointID = 0; //ID bezierObjList HIEN TAI
    public float speed = 2; // TOC DO DI CHUYEN
    public float reachDistance = 0.4f; // KHOANG CACH DAT DUOC

    float distance; // KHOANG CACH CUA ENEMY VOI bezierObjList
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

    // ENUM TRANG THAI CUA ENEMY
    public enum EnemyStates
    {
        ON_PATH,
        FLY_IN,
        IDLE,
        DIVE
    }

    // TRANG THAI ENEMY
    public EnemyStates enemyStates;
    public int enemyID; // ID CUA ENEMY 
    public Formation formation; // DOI HINH CUA ENEMY

    void Start()
    {
        StartCoroutine(Shoot());
    }

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

    /// <summary>
    /// DI CHUYEN ENEMY VAO FORMATION
    /// </summary>
    void MoveToFormation()
    {
        transform.position = Vector3.MoveTowards(transform.position, formation.GetVector(enemyID), speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, formation.GetVector(enemyID)) <= 0.0001f)
        {
            transform.SetParent(formation.gameObject.transform);
            transform.eulerAngles = Vector3.zero;

            // SAU KHI ENEMY VAO FORMATION THI THONG BAO CHO FORMATION 
            formation.enemyList.Add(new Formation.EnemyFormation(enemyID, transform.localPosition.x, transform.localPosition.y, this.gameObject));

            enemyStates = EnemyStates.IDLE;
        }
    }

    /// <summary>
    /// DI CHUYEN ENEMY THEO PATH
    /// </summary>
    /// <param name="path"></param>
    void MoveOnThePath(Path path)
    {
        distance = Vector3.Distance(path.bezierObjList[currentWayPointID], transform.position);
        transform.position = Vector3.MoveTowards(transform.position, path.bezierObjList[currentWayPointID], speed * Time.deltaTime);


        if (distance <= reachDistance)
        {
            currentWayPointID++;
        }

        if (currentWayPointID >= path.bezierObjList.Count)
        {
            currentWayPointID = 0;

            // DIVING
            if (enemyStates == EnemyStates.DIVE)
            {
                transform.position = GameObject.Find("EnemySpawnPoint").transform.position;
                Destroy(pathToFollow.gameObject);
            }

            enemyStates = EnemyStates.FLY_IN;
        }


    }

    /// <summary>
    /// THIET LAP DOI HINH
    /// </summary>
    /// <param name="path"></param>
    /// <param name="ID"></param>
    /// <param name="_formation"></param>
    public void SpawnSetup(Path path, int ID, Formation _formation)
    {
        pathToFollow = path;
        enemyID = ID;
        formation = _formation;
    }

    /// <summary>
    /// THIET LAP DIVING
    /// </summary>
    /// <param name="path"></param>
    public void DiveSetup(Path path)
    {
        pathToFollow = path;
        transform.SetParent(transform.parent.parent);
        enemyStates = EnemyStates.DIVE;
    }

    /// <summary>
    /// NHAN DAME TU BULLET
    /// </summary>
    /// <param name="amount"></param>
    public void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            //PLAY SOUND
            if (AudioManager.instance != null)
            {
                AudioManager.instance.PlaySFX(AudioManager.instance.explosionSound_Enemy, 0.2f);
            }
            // INSTATIATE PRATICLE
            GameObject enemyExplosion = Instantiate(enemyExplosionPrefab, transform.position, Quaternion.identity);
            Destroy(enemyExplosion, 0.4f);

            // ADD SCORE
            GamePlayController.instance.AddScore(score);

            //REPORT CHO FORMATION NHUNG ENEMY DA BI DESTROY
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

            Destroy(gameObject);
        }
    }


    public void SetDameAndHealth(int newDamage, int newHealth)
    {
        bulletDamage += newDamage;
        health += newHealth;
    }

    void Fire()
    {
        if (bullet != null && spawnPoint != null && enemyStates == EnemyStates.IDLE)
        {
            GameObject newBullet = Instantiate(bullet, spawnPoint.position, Quaternion.identity);
            newBullet.GetComponent<BulletController>().SetDamage(bulletDamage);
            //newBullet.gameObject.transform.SetParent(transform);
        }
    }
    IEnumerator Shoot()
    {
        yield return new WaitForSeconds(enemyBulletSpawnTime);
        Fire();
        StartCoroutine(Shoot());
    }
}
