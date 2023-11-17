using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{
    public Color pathColor = Color.green;
    Transform[] objArray;
    public List<Transform> pathObjList = new List<Transform>();
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    void OnDrawGizmos()
    {
        Gizmos.color = pathColor;
        objArray = GetComponentsInChildren<Transform>();
    }    

     
}
