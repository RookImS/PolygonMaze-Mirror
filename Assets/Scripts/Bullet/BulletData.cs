using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletData : MonoBehaviour
{
    public string bulletName;
    public BulletStatSystem stats;
    public TowerSkill towerSkill;
    public void Init(TowerStatSystem t_stat)
    {
        stats.SetBaseStat(t_stat);
        stats.Init(this);
    }
}
