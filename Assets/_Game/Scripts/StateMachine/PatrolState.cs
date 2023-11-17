using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PatrolState : IState
{
    public void OnEnter(Bot bot)
    {

    }

    public void OnExecute(Bot bot)
    {
        bot.Patrolling();
    }

    public void OnExit(Bot bot)
    {
    }
}
