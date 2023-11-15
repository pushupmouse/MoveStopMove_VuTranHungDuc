using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : IState
{
    public void OnEnter(Bot bot)
    {
        Debug.Log("PATROL");

    }

    public void OnExecute(Bot bot)
    {
        //moving around
        Debug.Log("MOVING AROUND");

        if(bot.Target != null)
        {
            Debug.Log("FOUND ENEMY AFTER PATROL");
            bot.ChangeState(new IdleState());
        }
    }

    public void OnExit(Bot bot)
    {
    }
}
