using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalData : BulletData
{
    public override void Init(TowerStatSystem t_stat)
    {
        stats.SetBaseStat(t_stat);
        stats.Init(this);
        transform.GetChild(1).GetComponent<SphereCollider>().radius = stats.stats.splashRange;
    }

    public override void Act()
    {
        Vector3 direction = transform.rotation * Vector3.forward;
        Vector3 moveSpeed = direction * stats.stats.speed;
        gameObject.GetComponent<Rigidbody>().velocity = moveSpeed;
    }

    public override IEnumerator CheckLiving()
    {
        float livingTime = stats.stats.attackRange / stats.stats.speed;

        yield return new WaitForSeconds(livingTime);

        Destroy(this.gameObject);
    }

    public override void Attack()
    {
        transform.GetChild(1).GetComponent<SphereCollider>().enabled = true;
    }
}
