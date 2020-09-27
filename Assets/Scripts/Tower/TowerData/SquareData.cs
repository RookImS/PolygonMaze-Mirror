using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareData : TowerData
{
    
    public override void Shoot(Transform muzzle)
    {
         GameObject bulletInstance = Instantiate(bullet, muzzle.position, muzzle.rotation);
         Debug.Log("Test1");
         bulletInstance.GetComponent<BulletData>().Init(Stats);
    }
}

