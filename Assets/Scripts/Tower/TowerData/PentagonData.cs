using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PentagonData : TowerData
{
    public override void Shoot(Transform muzzle)
    {
        GameObject bulletInstance = Instantiate(bullet, muzzle.position, muzzle.rotation);
        bulletInstance.GetComponent<BulletBehaviour>().Init(Stats, towerSkill);
        SoundManager.Instance.PlaySound(SoundManager.TowerSoundSpecific.PENTAGON, "Pentagon_Attack");
    }
}