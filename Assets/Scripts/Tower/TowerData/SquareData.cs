using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareData : TowerData
{
    public override void Shoot(Transform muzzle)
    {
        GameObject bulletInstance = Instantiate(bullet, muzzle.position, muzzle.rotation, bulletObject.transform);
        bulletInstance.GetComponent<BulletBehaviour>().Init(Stats);
        if (SoundManager.Instance != null)
            SoundManager.Instance.PlaySound(SoundManager.TowerSoundSpecific.HEXAGON, "Square_Attack");
    }
}
