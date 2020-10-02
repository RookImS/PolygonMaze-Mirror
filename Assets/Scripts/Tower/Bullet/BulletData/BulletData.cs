using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletData : MonoBehaviour
{
    public string bulletName;
    public BulletStatSystem stats;

    public virtual void Init(TowerStatSystem t_stat)
    {
    }
    public virtual void Init(TowerStatSystem t_stat, int ticRate, float attackDuration)
    {
    }

    public virtual void Act()
    {

    }
    public virtual void Attack()
    {

    }

    public virtual IEnumerator CheckLiving()
    {
        yield return 0;
    }
}
