using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : Singleton<BulletPool>
{
    [SerializeField] private Bullet bullet;
    [SerializeField] private int amountToPool = 50;

    private List<GameObject> pooledObjects = new List<GameObject>();

    private void Start()
    {
        for (int i = 0; i < amountToPool; i++)
        {
            GameObject pooledObject = Instantiate(bullet.prefab);
            pooledObject.SetActive(false);
            pooledObjects.Add(pooledObject);
        }
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }

        return null;
    }
}
