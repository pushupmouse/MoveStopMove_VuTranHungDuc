using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IState
{
    public void OnEnter(Bot bot)
    {
        Debug.Log("ATTACK");
        //stand and fire attack
    }

    public void OnExecute(Bot bot)
    {
        if(bot.Target != null)
        {
            Debug.Log("ATTACKING THE TARGET");
        }
        else
        {
            Debug.Log("TARGET ESCAPED");
            bot.ChangeState(new IdleState());
        }
    }

    public void OnExit(Bot bot)
    {
    }
}
