using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Green : FieldSkill
{
    List<GameObject> hitList;

    public int totalDamage;
    public int damageNumber;        // totalDamage를 적용시키기 위한 횟수
    
    public override void ApplySkill(GameObject obj)
    {
        if (hitList == null)
            hitList = new List<GameObject>();

        bool alreadyHit = false;

        for (int i = 0; i < hitList.Count; i++)
        {
            if (hitList[i] == null)
                continue;

            if (hitList[i] == obj)
            {
                alreadyHit = true;
                break;
            }
        }

        if (!alreadyHit)
        {
            modifier = new EnemyStatSystem.StatModifier();

            modifier.ModifierMode = EnemyStatSystem.StatModifier.Mode.Percentage;

            EnemyStatSystem.Stats changeStat = new EnemyStatSystem.Stats();

            changeStat.speed = 0;
            changeStat.hp = 0;
            changeStat.def = 0;

            modifier.Stats = changeStat;
            modifier.damage = totalDamage / damageNumber;
            modifier.applyDuration = applyDuration / damageNumber;

            obj.GetComponent<EnemyData>().Stats.AddTimedModifier(modifier, applyDuration, id, skillEffect, skillSoundSpecific, apply_sound_name);
            hitList.Add(obj);
        }

    }
}
