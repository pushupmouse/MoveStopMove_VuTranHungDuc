using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class AttackState : IState
{
    public void OnEnter(Bot bot)
    {
        
    }

    public void OnExecute(Bot bot)
    {
        bot.Attacking();
    }

    public void OnExit(Bot bot)
    {
    }
}
