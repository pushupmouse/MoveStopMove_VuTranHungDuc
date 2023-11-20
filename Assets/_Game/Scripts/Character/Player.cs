using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : Character
{
    private FloatingJoystick joystick;
    private Rigidbody rb;
    private CameraFollow _camera;
    private int levelToAdjust = 0;


    private void Awake()
    {
        joystick = FindObjectOfType<FloatingJoystick>();
        _camera = FindObjectOfType<CameraFollow>();
        my_collider = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
        level = 0;
    }

    private void FixedUpdate()
    {
        MoveWithJoystick();

        if (target != null && Vector3.Distance(transform.position, target.transform.position) > attackRange)
        {
            target = null;
        }

        if (target == null)
        {
            FindTarget();
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            LevelManager.instance.SpawnBot();
        }
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
            ChangeAnimation(MyConst.Animation.IDLE);
            PrepareAttack();
        }

        rb.MovePosition(rb.position + moveVector);
    }

    public override void Deactivate()
    {
        
    }

    public override void OnKill()
    {
        base.OnKill();

        levelToAdjust++;

        if(levelToAdjust >= 5 && level <= 15)
        {
            _camera.AdjustCamera();
            levelToAdjust = 0;
        }
    }
}
