using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Bot : Character
{
    private NavMeshAgent agent;
    private IState currentState;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        my_collider = GetComponent<Collider>();
        ChangeState(new IdleState());
    }

    private void FixedUpdate()
    {
        if (currentState != null)
        {
            currentState.OnExecute(this);
        }

        //float posX = (Random.Range(-transform.position.x, transform.position.x));
        //float posZ = (Random.Range(-transform.position.z, transform.position.z));

        //agent.SetDestination(new Vector3 (posX, transform.position.y, posZ));
        
        FindTarget();
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
