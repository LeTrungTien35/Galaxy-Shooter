using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public GameObject bullet;
    public Transform spawnPoin1;
    public Transform spawnPoin2;
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
        Instantiate(bullet, spawnPoin1.position, Quaternion.identity);
        Instantiate(bullet, spawnPoin2.position, Quaternion.identity);
    }    

    IEnumerator Shoot()
    {
        yield return new WaitForSeconds(tocDoBan);
        Fire();
        audioSource.Play();
        StartCoroutine(Shoot());
    }    
}
