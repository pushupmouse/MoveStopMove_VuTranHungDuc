using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] public Rigidbody rb;
    [SerializeField] public Transform mesh;

    public Character attacker;
    private float speed = 5f;
    private float rotateSpeed = 200f;
    private Vector3 startPoint;

    private void Update()
    {
        mesh.Rotate(Vector3.up * rotateSpeed * Time.deltaTime, Space.Self);

        if(Vector3.Distance(startPoint, transform.position) > attacker.attackRange)
        {
            Deactivate();
        }
    }

    public void Activate(Vector3 direction)
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = direction * speed;
        rb.transform.rotation = Quaternion.LookRotation(direction);
        gameObject.SetActive(true);

        startPoint = transform.position;
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8 && other.gameObject != attacker.gameObject)
        {
            Character character = other.GetComponent<Character>();
            character.OnHit();
            Deactivate();
        }
    }
}
