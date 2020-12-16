using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blue : FieldSkill
{
    public float speedDecreaseRate;

    public override void ApplySkill(GameObject obj)
    {
        modifier = new EnemyStatSystem.StatModifier();

        modifier.ModifierMode = EnemyStatSystem.StatModifier.Mode.Percentage;

        EnemyStatSystem.Stats changeStat = new EnemyStatSystem.Stats();

        changeStat.speed = speedDecreaseRate * -1;
        changeStat.hp = 0;
        changeStat.def = 0;

        modifier.Stats = changeStat;

        obj.GetComponent<EnemyData>().Stats.AddTimedModifier(modifier, applyDuration, id, skillEffect, skillSoundSpecific, apply_sound_name);
    }
}
