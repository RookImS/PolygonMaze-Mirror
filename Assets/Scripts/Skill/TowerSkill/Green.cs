using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Green : FieldSkill
{
    public int totalDamage;
    public int damageNumber;        // totalDamage를 적용시키기 위한 횟수
    
    public override void ApplySkill(GameObject obj)
    {
        obj.GetComponent<EnemyData>().Stats.Poison(totalDamage, damageNumber, applyDuration, id, skillSprite);
    }
}
