using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool instance;
    //dictionary(weapon type, pooled objects)
    private List<GameObject> pooledObjects = new List<GameObject>();
    
    [SerializeField] private GameObject prefab;
    [SerializeField] private int amountToPool = 50;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

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
        for(int i = 0;i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }
        
        return null;
    }

}
