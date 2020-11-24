using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Red : FieldSkill
{
    List<GameObject> hitList;
    public int damage;

    public override void ApplySkill(GameObject obj)
    {
        if(hitList == null)
            hitList = new List<GameObject>();

        bool alreadyHit = false;

        for(int i = 0; i < hitList.Count; i++)
        {
            if (hitList[i] == null)
                continue;

            if (hitList[i] == obj)
            {
                alreadyHit = true;
                break;
            }
        }

        if(!alreadyHit)
        {
            obj.GetComponent<EnemyData>().Damage(damage);
            hitList.Add(obj);
        }
    }
}
