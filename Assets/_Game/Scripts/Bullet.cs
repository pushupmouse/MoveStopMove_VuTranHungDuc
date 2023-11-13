using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] public Rigidbody rb;
    [SerializeField] public Transform mesh;

    public GameObject attacker;
    private float speed = 5f;
    private float rotateSpeed = 200f;

    private void Update()
    {
        mesh.Rotate(Vector3.up * rotateSpeed * Time.deltaTime, Space.Self);
    }

    public void Activate(Vector3 direction)
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = direction * speed;
        rb.transform.rotation = Quaternion.LookRotation(direction);
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8 && other.gameObject != attacker)
        {
            Debug.Log("hit " + other);
            Deactivate();
        }
    }
}
