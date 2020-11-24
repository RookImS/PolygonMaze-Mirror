using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldSkill : Skill
{
    [HideInInspector] public EnemyStatSystem.StatModifier modifier;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
                ApplySkill(other.gameObject);
                applyCountDown = applyInterval;
        }
    }
}
