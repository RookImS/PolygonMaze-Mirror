using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSystem
{
    public void ApplyWaveSystem(EnemyData enemyData)
    {
        int i = 0;
        
        for (i = 0; i < enemyData.GetWaveNum() - 1; i++)
        {
            enemyData.Stats.baseStats.Modify(enemyData.Stats.baseStatModifier);
        }
        enemyData.Init();
    }
}
