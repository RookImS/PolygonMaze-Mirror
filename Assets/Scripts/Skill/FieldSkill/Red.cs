using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Red : FieldSkill
{
    List<GameObject> hitList;
    public int damage;

    private float timeCheck = 0f;
    private float delayTime = 1.4f;
    private float affectEndTime = 1.8f;



    private void Update()
    {
        if (effectObject.activeSelf)
        {
            if (affectEndTime > timeCheck)
                timeCheck += Time.deltaTime;
        }
    }

    public override void ApplySkill(GameObject obj)
    {
        if (delayTime <= timeCheck && timeCheck <= affectEndTime)
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
                obj.GetComponent<EnemyBehaviour>().Damage(damage);
                hitList.Add(obj);
            }
        }
    }
}
