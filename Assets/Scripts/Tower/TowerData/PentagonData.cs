using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PentagonData : TowerData
{
    public override void Shoot(Transform muzzle)
    {
        GameObject bulletInstance = Instantiate(bullet, muzzle.position, muzzle.rotation, bulletObject.transform);
        bulletInstance.GetComponent<BulletBehaviour>().Init(Stats);
        if (SoundManager.Instance != null)
            SoundManager.Instance.PlaySound(SoundManager.TowerSoundSpecific.PENTAGON, "Pentagon_Attack");
    }

    public override void SetSkillEffect(GameObject effect)
    {
        GameObject newEffect = Instantiate(effect, transform);
           newEffect.transform.localScale *= 1.576f;
    }
}
