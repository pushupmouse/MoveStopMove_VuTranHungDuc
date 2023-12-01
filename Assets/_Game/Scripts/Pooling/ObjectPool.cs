using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : Singleton<ObjectPool>
{
    //dictionary(weapon type, pooled objects)

    [SerializeField] private GameObject prefab;
    [SerializeField] private int amountToPool = 50;

    private List<GameObject> pooledObjects = new List<GameObject>();

    private void Start()
    {
        for (int i = 0; i < amountToPool; i++)
        {
            GameObject pooledObject = Instantiate(prefab);
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
