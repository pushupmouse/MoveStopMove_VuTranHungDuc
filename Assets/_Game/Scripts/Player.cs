using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : Character
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private Transform throwPoint;
    [SerializeField] private LayerMask layerMask;

    private FloatingJoystick joystick;
    private Rigidbody rb;
    private Vector3 moveVector;
    private Vector3 direction;
    private float timer = 0f;
    private bool canAttack = false;
    private float attackRange = 5f;
    Collider[] enemiesInRange;
    GameObject target;

    private void Awake()
    {
        joystick = FindObjectOfType<FloatingJoystick>();
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
                    ExecuteAttack(throwPoint, direction);
                    timer = 0f;
                    canAttack = false;
                }   
            }
        }

        rb.MovePosition(rb.position + moveVector);
    }

    private void FindTarget()
    {
        int maxTargets = 10;
        enemiesInRange = new Collider[maxTargets]; 

        int numEnemies = Physics.OverlapSphereNonAlloc(transform.position, attackRange, enemiesInRange, layerMask);
        for (int i = 0; i < numEnemies; i++)
        {
            target = enemiesInRange[i].gameObject;
        }
    }

    public override void ExecuteAttack(Transform throwPoint, Vector3 direction)
    {
        base.ExecuteAttack(throwPoint, direction);
    }



}
