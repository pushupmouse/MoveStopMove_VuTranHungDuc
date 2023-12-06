using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : Singleton<BulletPool>
{
    [Serializable]
    public class WeaponObject
    {
        public WeaponType weaponType;
        public GameObject prefab;
    }

    [SerializeField] private List<WeaponObject> weaponObjects = new List<WeaponObject>();
    [SerializeField] private int amountToPool = 50;

    private Dictionary<WeaponType, List<GameObject>> pooledObjects = new Dictionary<WeaponType, List<GameObject>>();

    private void Start()
    {
        for (int i = 0; i < weaponObjects.Count; i++)
        {
            WeaponObject weaponObject = this.weaponObjects[i];
            List<GameObject> weaponObjects = new List<GameObject>();

            for (int j = 0; j < amountToPool; j++)
            {
                GameObject pooledObject = Instantiate(weaponObject.prefab);
                pooledObject.SetActive(false);
                weaponObjects.Add(pooledObject);
            }

            pooledObjects.Add(weaponObject.weaponType, weaponObjects);
        }
    }

    public GameObject GetPooledObject(WeaponType weaponType)
    {
        if (pooledObjects.ContainsKey(weaponType))
        {
            List<GameObject> weaponObjects = pooledObjects[weaponType];

            for (int i = 0; i < weaponObjects.Count; i++)
            {
                if (!weaponObjects[i].activeInHierarchy)
                {
                    return weaponObjects[i];
                }
            }
        }

        return null;
    }
}
