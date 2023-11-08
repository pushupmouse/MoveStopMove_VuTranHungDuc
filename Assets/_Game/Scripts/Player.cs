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

    private float timer = 0f;
    private bool canAttack = true;

    private void Awake()
    {
        joystick = FindObjectOfType<FloatingJoystick>();
        rb = GetComponent<Rigidbody>();

    }

    private void FixedUpdate()
    {
        MoveWithJoystick();
        //PrepareAttack(moveVector);
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

            timer = 0f;
            canAttack = true;

            ChangeAnimation("Run");
        }
        else if (joystick.Horizontal == 0 && joystick.Vertical == 0)
        {
            timer += Time.fixedDeltaTime;
            ChangeAnimation("Idle");
            if (timer > 1f)
            {
                if (canAttack)
                {
                    ChangeAnimation("Attack");
                    Invoke(nameof(ExecuteAttack), 0.25f);
                    timer = 0f;
                    canAttack = false;
                }
                
            }
        }

        rb.MovePosition(rb.position + moveVector);
    }

    public override void ExecuteAttack()
    {
        base.ExecuteAttack();
        Debug.Log("shoot");
        Rigidbody prefab = Instantiate(bullet, transform.position, transform.rotation);
        prefab.velocity = transform.forward * 20f;
       
    }

    

}
