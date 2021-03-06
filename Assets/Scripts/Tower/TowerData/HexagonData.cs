using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexagonData : TowerData
{
    public override void Init()
    {
        base.Init();
    }

    public override void Shoot(Transform muzzle)
    {
        
        bullet.GetComponent<SnipeBulletBehaviour>().Hit();

        if (SoundManager.Instance != null)
            SoundManager.Instance.PlaySound(SoundManager.TowerSoundSpecific.HEXAGON, "Hexagon_Attack");
    }

    public void ReloadBullet()
    {
        bullet.GetComponent<BulletBehaviour>().Init(Stats);
        bullet.SetActive(false);
        bullet.GetComponent<SnipeBulletBehaviour>().Reload();
    }

    public void LocateBullet(GameObject target)
    {
        if (!bullet.activeSelf)
            bullet.SetActive(true);

        bullet.transform.position = target.transform.position;
    }

    public override void SetSkillEffect(GameObject effect)
    {
        GameObject newEffect = Instantiate(effect, transform);
        newEffect.transform.localScale *= 1.932f;
    }
}
