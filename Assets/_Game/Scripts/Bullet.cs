using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] public Rigidbody rb;

    private void Awake()
    {

        
    }

    private void FixedUpdate()
    {
    }

    public void DestroyGameObject()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            gameObject.SetActive(false);
        }
    }
}
