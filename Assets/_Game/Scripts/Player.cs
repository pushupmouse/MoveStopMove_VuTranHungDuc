using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : Character
{
    private FloatingJoystick joystick;
    private Rigidbody rb;


    private void Awake()
    {
        joystick = FindObjectOfType<FloatingJoystick>();
        my_collider = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        MoveWithJoystick();
        FindTarget();
    }

    private void MoveWithJoystick()
    {
        moveVector = Vector3.zero;
        moveVector.x = joystick.Horizontal * moveSpeed * Time.fixedDeltaTime;
        moveVector.z = joystick.Vertical * moveSpeed * Time.fixedDeltaTime;

        if (joystick.Horizontal != 0 || joystick.Vertical != 0)
        {
            Moving();
        }
        else if (joystick.Horizontal == 0 && joystick.Vertical == 0)
        {
            PrepareAttack();
        }

        rb.MovePosition(rb.position + moveVector);
    }
}
