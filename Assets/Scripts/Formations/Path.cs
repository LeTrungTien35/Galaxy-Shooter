using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{
    [SerializeField]
    bool running; // NEU CHAY THI KHONG XOA LIST BEZIER

    public bool visualizePath; //

    // Mau cua Gizmos
    public Color pathColor = Color.green;

    // Mang chua gameobject con trong gameobj cha
    Transform[] objArray;

    // List them ds objarray vao
    public List<Transform> pathObjList = new List<Transform>();

    // mat do dong
    [Range(0, 20)]
    public int lineDensity = 1;
    int overload;
    //
    public List<Vector3> bezierObjList = new List<Vector3>();


    void Start()
    {
        CreatePath();
    }


    void Update()
    {

    }

    
    void OnDrawGizmos()
    {
        if (visualizePath)
        {
            // straight path
            Gizmos.color = pathColor;
            // fill the array
            objArray = GetComponentsInChildren<Transform>();
            //clearObj
            pathObjList.Clear();
            foreach (Transform obj in objArray)
            {
                if (obj != this.transform)
                {
                    pathObjList.Add(obj);
                }
            }

            // Draw the object
            for (int i = 0; i < pathObjList.Count; i++)
            {
                Vector3 positon = pathObjList[i].position;
                if (i > 0)
                {
                    Vector3 previous = pathObjList[i - 1].position;
                    Gizmos.DrawLine(previous, positon);
                    Gizmos.DrawWireSphere(positon, 0.2f);
                }
            }


            // CHECK OVERLOAD

            if (pathObjList.Count % 2 == 0)
            {
                pathObjList.Add(pathObjList[pathObjList.Count - 1]);
                overload = 2;
            }
            else
            {
                pathObjList.Add(pathObjList[pathObjList.Count - 1]);
                pathObjList.Add(pathObjList[pathObjList.Count - 1]);
                overload = 3;
            }

            //CURVE CREATING
            bezierObjList.Clear();
            Vector3 lineStart = pathObjList[0].position;

            for (int i = 0; i < pathObjList.Count - overload; i += 2)
            {
                for (int j = 0; j <= lineDensity; j++)
                {
                    Vector3 lineEnd = GetPoint(pathObjList[i].position, pathObjList[i + 1].position, pathObjList[i + 2].position, j / (float)lineDensity);

                    Gizmos.color = Color.red;
                    Gizmos.DrawLine(lineStart, lineEnd);

                    Gizmos.color = Color.blue;
                    Gizmos.DrawWireSphere(lineStart, 0.1f);

                    lineStart = lineEnd;
                    bezierObjList.Add(lineStart);
                }
            }
        }
        else
        if(!running)
        {
            pathObjList.Clear();
            bezierObjList.Clear();
        }
    }
    
    Vector3 GetPoint(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        return Vector3.Lerp(Vector3.Lerp(p0, p1, t), Vector3.Lerp(p1, p2, t), t);

    }


    void CreatePath()
    {
        objArray = GetComponentsInChildren<Transform>();
        //clearObj
        pathObjList.Clear();
        foreach (Transform obj in objArray)
        {
            if (obj != this.transform)
            {
                pathObjList.Add(obj);
            }
        }

        // CHECK OVERLOAD

        if (pathObjList.Count % 2 == 0)
        {
            pathObjList.Add(pathObjList[pathObjList.Count - 1]);
            overload = 2;
        }
        else
        {
            pathObjList.Add(pathObjList[pathObjList.Count - 1]);
            pathObjList.Add(pathObjList[pathObjList.Count - 1]);
            overload = 3;
        }

        // TAO CURVE 
        bezierObjList.Clear();
        Vector3 lineStart = pathObjList[0].position;

        for (int i = 0; i < pathObjList.Count - overload; i += 2)
        {
            for (int j = 0; j <= lineDensity; j++)
            {
                Vector3 lineEnd = GetPoint(pathObjList[i].position, pathObjList[i + 1].position, pathObjList[i + 2].position, j / (float)lineDensity);

                lineStart = lineEnd;
                bezierObjList.Add(lineStart);
            }
        }
    }
}
