using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : Character
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private Rigidbody bullet;

    private FloatingJoystick joystick;
    private Rigidbody rb;
    private Vector3 moveVector;
    private Vector3 direction;


    private void Awake()
    {
        joystick = FindObjectOfType<FloatingJoystick>();
        rb = GetComponent<Rigidbody>();

    }

    private void FixedUpdate()
    {
        MoveWithJoystick();
        PrepareAttack(moveVector);
    }

    private void MoveWithJoystick()
    {
        moveVector = Vector3.zero;
        moveVector.x = joystick.Horizontal * moveSpeed * Time.fixedDeltaTime;
        moveVector.z = joystick.Vertical * moveSpeed * Time.fixedDeltaTime;

        if (joystick.Horizontal != 0 || joystick.Vertical != 0)
        {
            direction = Vector3.RotateTowards(transform.forward, moveVector, rotateSpeed * Time.fixedDeltaTime, 0f);
            transform.rotation = Quaternion.LookRotation(direction);

            //animation run
        }
        else if (joystick.Horizontal == 0 && joystick.Vertical == 0)
        {
            //animation idle
        }

        rb.MovePosition(rb.position + moveVector);
    }

    public override void ExecuteAttack()
    {
        base.ExecuteAttack();
        Debug.Log("shoot");
        Rigidbody prefab = Instantiate(bullet, transform.position, transform.rotation);
        prefab.velocity = transform.forward * 5f;
    }

    

}
