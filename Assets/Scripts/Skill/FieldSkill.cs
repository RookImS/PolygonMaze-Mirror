using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldSkill : Skill
{
    private void OnTriggerStay(Collider other)
    {
        if (applyCountDown <= 0f)
        {
            if (other.gameObject.CompareTag("Enemy"))
                ApplySkill(other.gameObject);
            applyCountDown = applyInterval;
        }
    }
}
