using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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

    private void Start()
    {
        startPositon = transform.position;
        curPosX = transform.position.x;
        CreateGrid();
    }

    private void Update()
    {
        curPosX += Time.deltaTime * speed * direction;
        if(curPosX >= maxMoveOffset)
        {
            direction *= -1;
            curPosX = maxMoveOffset;
        }
        else if(curPosX <= -maxMoveOffset)
        {
            direction *= -1;
            curPosX = -maxMoveOffset;
        }

        transform.position = new Vector3(curPosX, startPositon.y, startPositon.z);
    }

    //private void OnDrawGizmos()
    //{
    //    gridList.Clear();

    //    int num = 0;

    //    for (int i = 0; i < gridSizeX; i++)
    //    {
    //        for (int j = 0; j < gridSizeY; j++)
    //        {
    //            float x = (gridOffsetX + gridOffsetX * 2 * (num / div)) * Mathf.Pow(-1, num % 2 + 1);
    //            float y = gridOffsety * ((num % div) / 2);

    //            Vector3 vec = new Vector3(this.transform.position.x + x, this.transform.position.y + y, 0);

    //            Handles.Label(vec, num.ToString());

    //            num++;

    //            gridList.Add(vec);
    //        }
    //    }
    //}

    //private void OnDrawGizmos()
    //{
    //    int num = 0;
    //    CreateGrid();
    //    foreach(Vector3 pos in gridList)
    //    {
    //        Gizmos.DrawWireSphere(GetVector(num), 0.1f);
    //        num++;
    //    }
    //}
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
}
