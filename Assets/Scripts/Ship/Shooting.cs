using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public Transform spawnPoin1;
    public Transform spawnPoin2;
    public Transform spawnPoin3;
    public float tocDoBan = 1f;
    public AudioSource audioSource;
    void Start()
    {
        StartCoroutine(Shoot());
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Fire()
    {

        GameObject bullet = ObjectPool.ins.GetPooledObject();       
        if (bullet != null)
        {
            bullet.transform.position = spawnPoin3.position;
        }      
    }    

    IEnumerator Shoot()
    {
        yield return new WaitForSeconds(tocDoBan);
        Fire();
        audioSource.Play();
        StartCoroutine(Shoot());
    }    
}
