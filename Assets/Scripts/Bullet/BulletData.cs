using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletData : MonoBehaviour
{
    public string bulletName;
    public BulletStatSystem stats;
    public TowerSkill towerSkill;
    public void Init(TowerStatSystem t_stat, TowerSkill t_skill)
    {
        stats.SetBaseStat(t_stat);
        stats.Init(this);

        towerSkill = t_skill;

        if(towerSkill != null)
        {
            Debug.Log("check");
            gameObject.AddComponent(towerSkill.GetType());
        }
    }
}
