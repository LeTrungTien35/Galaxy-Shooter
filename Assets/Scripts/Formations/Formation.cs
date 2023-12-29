using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Formation : MonoBehaviour
{
    // So luong cot (x)
    public int gridSizeX = 10;
    // So luong hang (y)
    public int gridSizeY = 2;

    //khoang cach giua cac diem X, Y
    public float gridOffsetX = 1f;
    public float gridOffsety = 1f;
    public int div = 4;

    // LIST CHUA VI TRI FORMATION
    public List<Vector3> gridList = new List<Vector3>();

    // DI CHUYEN FORMATION
    public float maxMoveOffset = 5f; // VI TRI TOI DA DI CHUYEN DUOC
    float curPosX; // VI TRI TRUC X 
    Vector3 startPositon; // VI TRI BAT DAU
    public float speed = 1f; // TOC DO
    int direction = -1; // HUONG DI CHUYEN

    //SPREADING (lan truyen doi hinh)
    bool canSpread; // DUOC PHEP SPREAD
    bool spreadStarted; // BAT DAU
    float curSpread; // 0 - 1
    float spreadSpeed = 0.5f; // TOC DO SPREAD
    int spreadDir = 1; // HUONG SPREAD

    // TAO LIST CHUA CLASS ENEMY FORMATION 
    //[HideInInspector]
    [SerializeField]
    public List<EnemyFormation> enemyList = new List<EnemyFormation>();

    // LIST PATH DIVING
    public List<GameObject> divePathList = new List<GameObject> ();

    

    [System.Serializable]
    public class EnemyFormation
    {
        public int index;
        public GameObject enemy;

        public Vector3 goal;
        public Vector3 start;

        public EnemyFormation(int index, float xPos, float yPos, GameObject enemy)
        {
            this.index = index; 
            //this.xPos = xPos;
            //this.yPos = yPos;
            this.enemy = enemy;

            start = new Vector3(xPos, yPos, 0);
            goal = new Vector3(xPos + (xPos * 0.3f), yPos, 0);
        }
                

    }

    private void Start()
    {
        startPositon = transform.position;
        curPosX = transform.position.x;
        CreateGrid();
    }

    private void Update()
    {
        // DI CHUYEN FORMATION TRAI PHAI
        if (!canSpread && !spreadStarted)
        {
            curPosX += Time.deltaTime * speed * direction;
            if (curPosX >= maxMoveOffset)
            {
                direction *= -1;
                curPosX = maxMoveOffset;
            }
            else if (curPosX <= -maxMoveOffset)
            {
                direction *= -1;
                curPosX = -maxMoveOffset;
            }
            transform.position = new Vector3(curPosX, startPositon.y, startPositon.z);
        }

        // SPREAD FORMATION
        if (canSpread)
        {
            curSpread += Time.deltaTime * spreadDir * spreadSpeed;

            if (curSpread >= 1 || curSpread <= 0)
            {
                // CHANGE SPREAD DIRECTION
                spreadDir *= -1;
            }

            for (int i = 0; i < enemyList.Count; i++)
            {
                if (Vector3.Distance(enemyList[i].enemy.transform.position, enemyList[i].goal) >= 0.001f)
                {
                    enemyList[i].enemy.transform.position = Vector3.Lerp(transform.position + enemyList[i].start, transform.position + enemyList[i].goal, curSpread);
                }
            }
        }   
    }

           
    // IEnumerator SPREAD AND DIVING
    public IEnumerator ActivateSpread()
    {
        // KIEM TRA XEM DA BAT DAU SPREAD CHUA
        if(spreadStarted)
        {
            yield break;
        }
        spreadStarted = true;

        // DI CHUYEN FORMATION DEN VI TRI BAN DAU
        while(transform.position.x != startPositon.x)
        {            
            transform.position = Vector3.MoveTowards(transform.position, startPositon, speed * Time.deltaTime);
            yield return null;
        }
        // CHO PHEP SPREAD
        canSpread = true;
        // GOI PHUONG THUC DIVING SAU 3 DEN 10S
        Invoke("SetDiving", Random.Range(3f, 10f));
    }    

    /*
    private void OnDrawGizmos()
    {
        gridList.Clear();

        int num = 0;

        for (int i = 0; i < gridSizeX; i++)
        {
            for (int j = 0; j < gridSizeY; j++)
            {
                float x = (gridOffsetX + gridOffsetX * 2 * (num / div)) * Mathf.Pow(-1, num % 2 + 1);
                float y = gridOffsety * ((num % div) / 2);

                Vector3 vec = new Vector3(this.transform.position.x + x, this.transform.position.y + y, 0);

                Handles.Label(vec, num.ToString());

                num++;

                gridList.Add(vec);
            }
        }
    }
    */

    private void OnDrawGizmos()
    {
        int num = 0;
        CreateGrid();
        foreach (Vector3 pos in gridList)
        {
            Gizmos.DrawWireSphere(GetVector(num), 0.1f);
            num++;
        }
    }

    /// <summary>
    /// TAO FRORMATION
    /// </summary>
    void CreateGrid()
    {
        gridList.Clear();

        int num = 0;

        for (int i = 0; i < gridSizeX; i++)
        {
            for (int j = 0; j < gridSizeY; j++)
            {
                float x = (gridOffsetX + gridOffsetX * 2 * (num / div)) * Mathf.Pow(-1, num % 2 + 1);
                float y = gridOffsety * ((num % div) / 2);

                Vector3 vec = new Vector3(x, y, 0);
                num++;

                gridList.Add(vec);
            }
        }
    }

    /// <summary>
    /// LAY VI TRI CUA FORMATION
    /// </summary>
    /// <param name="ID"></param>
    /// <returns></returns>
    public Vector3 GetVector(int ID)
    {
        return transform.position + gridList[ID];
    }

    /// <summary>
    ///  DIVING ENEMY
    /// </summary>
    void SetDiving()
    {
        if(enemyList.Count > 0)
        {
            int choosenPath = Random.Range(0, divePathList.Count);
            int choosenEnemy = Random.Range(0, enemyList.Count);

            GameObject newPath = Instantiate(divePathList[choosenPath],enemyList[choosenEnemy].start + transform.position, Quaternion.identity);

            enemyList[choosenEnemy].enemy.GetComponent<EnemyMoving>().DiveSetup(newPath.GetComponent<Path>());
            enemyList.RemoveAt(choosenEnemy);
            Invoke("SetDiving", Random.Range(3f, 10f));
        }
        else
        {
            CancelInvoke("SetDiving");
            return;
        }
    }    
}
