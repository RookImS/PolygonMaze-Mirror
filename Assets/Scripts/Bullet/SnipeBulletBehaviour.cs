using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnipeBulletBehaviour : BulletBehaviour
{
    public GameObject firstTargetEffect;
    public GameObject targetEffect;
    public bool isFirstAttack;

    private GameObject attackEffect;

    private void Update()
    {
        if (enemyList.Count > 0)
            Attack();
    }

    public override void Init(TowerStatSystem t_stat)
    {
        base.Init(t_stat);
        isFirstAttack = true;
    }

    public void Hit()
    {
        hitCollider.SetActive(true);
    }

    public void Reload()
    {
        if (attackEffect != null)
        {
            attackEffect.transform.SetParent(bulletEffectGameObject.transform);

            var main = attackEffect.GetComponent<ParticleSystem>().main;
            main.stopAction = ParticleSystemStopAction.Destroy;
        }

        firstTargetEffect.SetActive(false);
        targetEffect.SetActive(false);

        firstTargetEffect.SetActive(true);
        enemyList.Clear();
    }

    protected override void Attack()
    {
        foreach (GameObject enemy in enemyList)
        {
            if (enemy == null)
                continue;

            enemy.GetComponent<EnemyBehaviour>().Damage(m_BulletData.stats.stats.damage + enemy.GetComponent<EnemyData>().Stats.stats.def);
            Vector3 pos = new Vector3(enemy.transform.position.x, enemy.transform.position.y + 1f, enemy.transform.position.z);
            Vector3 rot = transform.rotation.eulerAngles;
        }

        hitCollider.SetActive(false);

        targetEffect.SetActive(true);

        enemyList.Clear();

        if (attackEffect == null)
        {
            attackEffect = Instantiate(VFX, transform);
            attackEffect.transform.localPosition += new Vector3(0f, 1f, 0f);
        }
        else
        {
            attackEffect.SetActive(true);
        }
    }
}
