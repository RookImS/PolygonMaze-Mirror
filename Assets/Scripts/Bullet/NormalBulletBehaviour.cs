using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalBulletBehaviour : BulletBehaviour
{
    private void Start()
    {
        StartCoroutine(CheckRange());
        SetMovement();
    }

    private void Update()
    {
        if(enemyList.Count > 0)
            Attack();

    }

    public override void Init(TowerStatSystem t_stat)
    {
        m_BulletData.Init(t_stat);
        hitCollider.GetComponent<SphereCollider>().radius = m_BulletData.stats.stats.splashRange;
    }

    private IEnumerator CheckRange()
    {
        float livingTime = m_BulletData.stats.stats.attackRange / m_BulletData.stats.stats.speed;

        yield return new WaitForSeconds(livingTime);

        Destroy(this.gameObject);
    }

    private void SetMovement()
    {
        Vector3 direction = transform.rotation * Vector3.forward;
        Vector3 moveSpeed = direction * m_BulletData.stats.stats.speed;
        gameObject.GetComponent<Rigidbody>().velocity = moveSpeed;
    }
    
    protected override void Attack()
    {
        foreach (GameObject enemy in enemyList)
        {
            if (enemy == null)
                continue;

            enemy.GetComponent<EnemyBehaviour>().Damage(m_BulletData.stats.stats.damage);
            Vector3 pos = new Vector3(enemy.transform.position.x, enemy.transform.position.y + 2, enemy.transform.position.z);
            Vector3 rot = transform.rotation.eulerAngles;

            rot = new Vector3(rot.x, rot.y + 180, rot.z);
            Instantiate(VFX, pos, Quaternion.Euler(rot));
        }
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (TagManager.Instance.isEnemyTag(other.gameObject))
        {
            hitCollider.SetActive(true);
        }
    }
}
