using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yellow : TowerSkill
{
    public int damageIncreaseRate;

    public override void ApplySkill(GameObject obj)
    {
        modifier = new TowerStatSystem.StatModifier();

        modifier.ModifierMode = TowerStatSystem.StatModifier.Mode.Percentage;

        TowerStatSystem.Stats changeStat = new TowerStatSystem.Stats();

        changeStat.damage = damageIncreaseRate;
        changeStat.aheadDelay = 0;
        changeStat.attackRate = 0;
        changeStat.recogRange = 0;
        changeStat.attackRange = 0;
        changeStat.splashRange = 0;

        modifier.Stats = changeStat;

        obj.GetComponent<TowerData>().Stats.AddTimedModifier(modifier, applyDuration, id, VFX, skillSoundSpecific, apply_sound_name);
        
    }
}
