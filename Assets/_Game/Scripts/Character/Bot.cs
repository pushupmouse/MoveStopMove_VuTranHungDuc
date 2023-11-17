using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Bot : Character
{
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float walkPointRange;

    Vector3 walkPoint;
    private NavMeshAgent agent;
    private IState currentState;
    private bool walkPointSet = false;


    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        my_collider = GetComponent<Collider>();
    }

    private void FixedUpdate()
    {
        if (currentState != null)
        {
            currentState.OnExecute(this);
        }
        
        FindTarget();

        if(target == null)
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
            agent.SetDestination(walkPoint);
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

        PrepareAttack();

        transform.LookAt(target.transform);
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
}
