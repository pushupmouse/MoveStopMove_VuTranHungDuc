using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bot : Character
{
    private IState currentState;

    private void Awake()
    {
        ChangeState(new IdleState());
    }

    private void FixedUpdate()
    {
        if (currentState != null)
        {
            currentState.OnExecute(this);
        }

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
