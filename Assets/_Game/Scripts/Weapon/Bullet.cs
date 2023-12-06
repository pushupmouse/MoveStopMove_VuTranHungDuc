using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using Lean.Pool;

public class Bullet : MonoBehaviour
{
    private Vector3 startPoint;
    internal Character attacker;
    public float bulletScaleBonus = 0.1f;
    public Rigidbody rb;
    public Transform mesh;
    public GameObject prefab;
    public float speed = 5f;
    public float rotateSpeed = 500f;

    private void Update()
    {
        mesh.Rotate(Vector3.up * rotateSpeed * Time.deltaTime, Space.World);

        if(Vector3.Distance(startPoint, transform.position) > attacker.attackRange)
        {
            Deactivate();
        }
    }

    public void Activate(Vector3 direction)
    { 
        rb.velocity = direction * speed;
        rb.transform.rotation = Quaternion.LookRotation(direction);
        gameObject.SetActive(true);

        startPoint = transform.position;
    }

    public void Deactivate()
    {
        transform.localScale = Vector3.one;
        speed = 5f;
        rotateSpeed = 500f;
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8 && other.gameObject != attacker.gameObject)
        {
            Character character = Cache.GetCharacter(other);

            character.OnHit();
            attacker.OnKill();
            Deactivate();
        }
    }
}
