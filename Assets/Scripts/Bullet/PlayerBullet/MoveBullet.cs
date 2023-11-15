using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBullet : MonoBehaviour
{
    public float tocDo = 10f;
    public Vector3 huongDiChuyen = Vector3.up;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(huongDiChuyen * tocDo * Time.deltaTime);
    }
}
