using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Formation : MonoBehaviour
{
    // So luong cot (x)
    public int gridSizeX = 10;
    // So luong hang (y)
    public int gridSizeY = 2;

    //khoang cach giua cac diem
    public float gridOffsetX = 1f;
    public float gridOffsety = 1f;

    public int div = 4;

    public List<Vector3> gridList = new List<Vector3>();

    // MOVE THE FORMATION
    public float maxMoveOffset = 5f;

    float curPosX; //moving position
    Vector3 startPositon;
    public float speed = 1f;
    int direction = -1;

    //SPREADING
    bool canSpread;
    bool spreadStarted;

    float spreadAmount = 1f;
    float curSpread;
    float spreadSpeed = 0.5f;
    int spreadDir = 1;

    //DIVING
    bool canDive;
    public List<GameObject> divePathList = new List<GameObject> ();

    [HideInInspector]
    public List<EnemyFormation> enemyList = new List<EnemyFormation> ();

    [System.Serializable]
    public class EnemyFormation
    {
        public int index;
        public float xPos;
        public float yPos;
        public GameObject enemy;

        public Vector3 goal;
        public Vector3 start;

        public EnemyFormation(int index, float xPos, float yPos, GameObject enemy)
        {
            this.index = index; 
            this.xPos = xPos;
            this.yPos = yPos;
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

        if (canSpread)
        {
            curSpread += Time.deltaTime * spreadDir * spreadSpeed;
            if (curSpread >= spreadAmount || curSpread <= 0)
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

        //if(canDive)
        //{
        //    canDive = false;
        //}      
    }

           

    public IEnumerator ActivateSpread()
    {
        if(spreadStarted)
        {
            yield break;
        }
        spreadStarted = true;

        while(transform.position.x != startPositon.x)
        {
            transform.position = Vector3.MoveTowards(transform.position, startPositon, speed * Time.deltaTime);
            yield return null;
        }
        canSpread = true;
        //canDive = true;
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

    public Vector3 GetVector(int ID)
    {
        return transform.position + gridList[ID];
    }

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
