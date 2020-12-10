using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnipeBulletBehaviour : BulletBehaviour
{
    public GameObject FirstTargetEffect;
    public GameObject TargetEffect;
    public GameObject AttackEffect;
    public bool isFirstAttack;

    public override void Init(TowerStatSystem t_stat)
    {
        m_BulletData.Init(t_stat);
        hitCollider.GetComponent<SphereCollider>().radius = m_BulletData.stats.stats.splashRange;

        isFirstAttack = true;
    }

    private void Update()
    {
        if (enemyList.Count > 0)
            Attack();
    }

    public void Hit()
    {
        hitCollider.SetActive(true);
    }

    public void Reload()
    {
        FirstTargetEffect.SetActive(true);
        TargetEffect.SetActive(false);
        isFirstAttack = true;
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

            //Instantiate(VFX, pos, Quaternion.Euler(rot));
        }

        enemyList.Clear();
        isFirstAttack = false;

        hitCollider.SetActive(false);

        FirstTargetEffect.SetActive(false);
        TargetEffect.SetActive(true);
        AttackEffect.SetActive(true);
    }
}
