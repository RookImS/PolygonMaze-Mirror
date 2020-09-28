using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexagonData : TowerData
{
    public int ticRate;
    public float attackDuration;

    public override void Shoot(Transform muzzle)
    {
        GameObject bulletInstance = Instantiate(bullet, muzzle.position, muzzle.rotation);
        Debug.Log("Test1");
        bulletInstance.GetComponent<BulletData>().Init(Stats);
    }
}
