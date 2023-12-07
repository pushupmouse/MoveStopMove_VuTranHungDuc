using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class IdleState : IState
{
    public void OnEnter(Bot bot)
    {
        bot.ChangeAnimation(MyConst.Animation.IDLE);
    }

    public void OnExecute(Bot bot)
    {
    }

    public void OnExit(Bot bot)
    {
    }
}
