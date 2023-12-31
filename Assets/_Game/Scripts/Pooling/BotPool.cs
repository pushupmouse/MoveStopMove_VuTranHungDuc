using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotPool : Singleton<BotPool>
{

    private List<GameObject> pooledObjects = new List<GameObject>();

    [SerializeField] private GameObject botPrefab;
    [SerializeField] public int amountToPool = 5;

    private void Start()
    {
        for (int i = 0; i < amountToPool; i++)
        {
            GameObject pooledObject = Instantiate(botPrefab);
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
