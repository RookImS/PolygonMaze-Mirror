using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSkill : Skill
{
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Tower"))
        {
            if (applyCountDown <= 0f)
            {
                ApplySkill(other.gameObject);
                applyCountDown = applyInterval;
            }
        }
    }
}
