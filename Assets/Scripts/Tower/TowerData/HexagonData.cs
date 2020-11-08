using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexagonData : TowerData
{
    public int ticRate;
    public float attackDuration;

    public override void Init()
    {
        Stats.Init(this);
        Stats.stats.attackRate = 1f / (1f / Stats.stats.attackRate + attackDuration);
        towerSkill = null;
    }
    public override void Shoot(Transform muzzle)
    {
        GameObject bulletInstance = Instantiate(bullet, muzzle.position, muzzle.rotation);
        bulletInstance.GetComponent<BulletBehaviour>().Init(Stats, ticRate, attackDuration);
    }
}