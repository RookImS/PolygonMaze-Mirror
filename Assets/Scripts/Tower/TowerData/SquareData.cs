using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareData : TowerData
{
    public override void Shoot(Transform muzzle)
    {
        GameObject bulletInstance = Instantiate(bullet, muzzle.position, muzzle.rotation);
        bulletInstance.GetComponent<BulletBehaviour>().Init(Stats);
        SoundManager.Instance.PlaySound(SoundManager.TowerSoundSpecific.HEXAGON, "Square_Attack");
    }
}
