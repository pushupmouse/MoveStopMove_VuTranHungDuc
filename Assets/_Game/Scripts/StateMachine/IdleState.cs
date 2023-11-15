using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class IdleState : IState
{
    public void OnEnter(Bot bot)
    {
        Debug.Log("IDLE");
    }

    public void OnExecute(Bot bot)
    {
        if (bot.Target != null)
        {
            Debug.Log("FOUND ONE");

            bot.ChangeState(new AttackState());

        }
        else
        {
            Debug.Log("No enemies found");

            bot.ChangeState(new PatrolState());
        }
    }

    public void OnExit(Bot bot)
    {
    }
}
