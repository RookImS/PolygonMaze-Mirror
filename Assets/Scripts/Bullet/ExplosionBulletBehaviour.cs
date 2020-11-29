using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionBulletBehaviour : BulletBehaviour
{
    private void Start()
    {
        StartCoroutine(DestroyWrongAttack());
    }

    private void Update()
    {
        if (enemyList.Count > 0)
            Attack();
    }

    public override void Init(TowerStatSystem t_stat, TowerSkill t_skill)
    {
        m_BulletData.Init(t_stat, t_skill);
        transform.localScale = new Vector3(m_BulletData.stats.stats.attackRange * 2, 1, m_BulletData.stats.stats.attackRange * 2);
    }
    private IEnumerator DestroyWrongAttack()
    {
        yield return new WaitForSeconds(0.5f);

        Destroy(this.gameObject);
    }

    protected override void Attack()
    {
        foreach (GameObject enemy in enemyList)
        {
            if (enemy == null)
                continue;

            enemy.GetComponent<EnemyBehaviour>().Damage(m_BulletData.stats.stats.damage);
        }
        Destroy(this.gameObject);
    }
}
