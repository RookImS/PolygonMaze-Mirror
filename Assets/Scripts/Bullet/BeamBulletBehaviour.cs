using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamBulletBehaviour : BulletBehaviour
{
    private int ticRate;
    private float attackDuration;

    private float ticCountDown;

    private void Start()
    {
        StartCoroutine(CheckDuration());
        ReadyToShoot();
        ticCountDown = 0f;
    }

    private void Update()
    {
        if (ticCountDown <= 0f)
        {
            Attack();
            ticCountDown = 1f / ticRate;
        }
        ticCountDown -= Time.deltaTime;
    }

    public override void Init(TowerStatSystem t_stat, TowerSkill t_skill, int ticRate, float attackDuration)    
    {
        m_BulletData.Init(t_stat, t_skill);
        transform.localScale = new Vector3(m_BulletData.stats.stats.splashRange / 5f * 2f, 1, m_BulletData.stats.stats.attackRange);
        this.ticRate = ticRate;
        this.attackDuration = attackDuration;
    }

    private IEnumerator CheckDuration()
    {
        yield return new WaitForSeconds(attackDuration);

        Destroy(this.gameObject);
    }

    private void ReadyToShoot()
    {
        Vector3 direction = transform.rotation * Vector3.forward;
        Vector3 targetPoint = direction * m_BulletData.stats.stats.attackRange / 2f;
        transform.position += targetPoint;
    }

    protected override void Attack()
    {
        foreach (GameObject enemy in enemyList)
        {
            if (enemy == null)
                continue;

            enemy.GetComponent<EnemyBehaviour>().Damage(m_BulletData.stats.stats.damage);
        }
    }

}
