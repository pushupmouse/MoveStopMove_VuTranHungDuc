using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void Awake()
    {
        Invoke(nameof(DestroyGameObject), 2f);
    }

    private void DestroyGameObject()
    {
        Destroy(gameObject);
    }
}
