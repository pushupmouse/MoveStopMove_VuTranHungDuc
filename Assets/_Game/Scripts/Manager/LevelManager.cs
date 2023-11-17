using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        Invoke(nameof(SpawnBot), 1f);
    }

    public void SpawnBot()
    {
        BotPool botPool = BotPool.instance;
        
        for(int i = 0; i < botPool.amountToPool; i++)
        {
            GameObject botObj = botPool.GetPooledObject();

            if (botObj != null)
            {
                Bot bot = botObj.GetComponent<Bot>();
                bot.Activate();
            }
        }
    }
}
