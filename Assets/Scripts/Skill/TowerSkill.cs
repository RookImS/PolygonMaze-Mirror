using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSkill : Skill
{
    [HideInInspector] public TowerStatSystem.StatModifier modifier;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Tower"))
        {
                ApplySkill(other.gameObject);
                //applyCountDown = applyInterval;
        }
    }
}
