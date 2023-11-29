using System;
using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Bot : Character
{
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float walkPointRange;
    
    public GameObject targetIndicator;
    Vector3 walkPoint;
    private NavMeshAgent agent;
    private IState currentState;
    private bool walkPointSet = false;



    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        my_collider = GetComponent<Collider>();
        Level = 0;
    }

    private void FixedUpdate()
    {
        if (currentState != null)
        {
            currentState.OnExecute(this);
        }

        if (target != null && Vector3.Distance(transform.position, target.transform.position) > attackRange)
        {
            target = null;
        }

        if (target == null)
        {
            FindTarget();
        }
        
        if (target == null)
        {
            ChangeState(new PatrolState());
        }
        else
        {
            ChangeState(new AttackState());
        }
    }

    internal void Patrolling()
    {
        if (!walkPointSet)
        {
            SearchWalkPoint();
        }
        else
        {
            if (gameObject.activeSelf)
            {
                agent.SetDestination(walkPoint);
            }
            Moving();
        }

        if((transform.position - walkPoint).magnitude < 1f)
        {
            walkPointSet = false;
        }
    }


    internal void Attacking()
    {
        agent.SetDestination(transform.position);
        ChangeAnimation(MyConst.Animation.IDLE);
        PrepareAttack();

        if(target != null)
        {
            transform.LookAt(target.transform);
        }
    }

    public override void OnHit()
    {
        base.OnHit();
        Invoke(nameof(Activate), 2f);
    }

    private void SearchWalkPoint()
    {
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        float randomZ = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, groundLayer))
        {
            walkPointSet = true;
        }
    }

    public void ChangeState(IState newState)
    {
        if (currentState != null)
        {
            currentState.OnExit(this);
        }

        currentState = newState;

        if (currentState != null)
        {
            currentState.OnEnter(this);
        }
    }

    public override void Activate()
    {
        base.Activate();

        float randomX = Random.Range(-15, 15);
        float randomZ = Random.Range(-15, 15);
        transform.position = new Vector3(randomX, transform.position.y, randomZ);
        isDead = false;
        gameObject.SetActive(true);
    }

    public override void Deactivate()
    {
        base.Deactivate();
    }

    public override void OnKill()
    {

    }
}
