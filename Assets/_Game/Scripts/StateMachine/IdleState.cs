using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class IdleState : IState
{
    public void OnEnter(Bot bot)
    {

    }

    public void OnExecute(Bot bot)
    {
        if (bot.Target != null)
        {
            bot.ChangeState(new AttackState());

        }
        else
        {
            bot.ChangeState(new PatrolState());
        }
    }

    public void OnExit(Bot bot)
    {
    }
}
