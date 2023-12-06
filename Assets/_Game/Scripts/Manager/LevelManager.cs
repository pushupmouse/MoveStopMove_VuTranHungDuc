using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    public void SpawnBots()
    {
        BotPool botPool = BotPool.Instance;

        for (int i = 0; i < botPool.amountToPool; i++)
        {
            GameObject botObj = botPool.GetPooledObject();

            if (botObj != null)
            {
                Bot bot = botObj.GetComponent<Bot>();
                bot.Activate();
            }
        }
    }

    public void Respawn(Bot bot)
    {
        bot.Activate();
    }
}
