using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool ins;   
    [SerializeField]
    private int amountToPool = 10;
    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private List<GameObject> pooledObject = new List<GameObject>();

    private void Awake()
    {
        if(ins == null)
        {
            ins = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        AddObjectToPool(amountToPool);
    }

    private void AddObjectToPool(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject go = Instantiate(bulletPrefab);
            go.SetActive(false);
            pooledObject.Add(go);
            go.transform.parent = transform;
        }
    }    
    public GameObject GetPooledObject()
    {
        for(int i = 0; i < pooledObject.Count; i++)
        {
            if(!pooledObject[i].activeSelf)
            {
                pooledObject[i].SetActive(true);
                return pooledObject[i];
            }
        }
        AddObjectToPool(1);
        pooledObject[pooledObject.Count - 1].SetActive(true);
        return pooledObject[pooledObject.Count - 1];
    }
}
