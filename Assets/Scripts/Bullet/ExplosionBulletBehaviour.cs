using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionBulletBehaviour : BulletBehaviour
{
    public float life;

    private float attackStartTime;
    private float age;
    private float attackTime;
    private int attackNumber;

    private new void Awake()
    {
        m_BulletData = GetComponent<BulletData>();
        enemyList = new List<GameObject>();

        attackStartTime = 0.3f;
        age = 0f;
        attackTime = 0.7f;
        attackNumber = 0;
    }
    private void Start()
    {
        StartCoroutine(DestroyAttack());
    }

    private void Update()
    {
        if (age < attackTime)
        {
            age += Time.deltaTime;
            if (attackStartTime < age)
            {
                if (enemyList.Count > 0)
                    Attack();
                if (age >= attackTime)
                    hitCollider.SetActive(false);
            }
        }
    }

    public override void Init(TowerStatSystem t_stat)
    {
        m_BulletData.Init(t_stat);
        transform.localScale = new Vector3(m_BulletData.stats.stats.attackRange * 2, 1, m_BulletData.stats.stats.attackRange * 2);
        bulletEffectGameObject = GameObject.Find("BulletEffect");
    }
    private IEnumerator DestroyAttack()
    { 
        yield return new WaitForSeconds(life);
        Destroy(this.gameObject);
    }

    protected override void Attack()
    {
        if (attackNumber != enemyList.Count)
        {
            for (int i = attackNumber; i < enemyList.Count; i++)
            {
                attackNumber++;
                if (enemyList[i] == null)
                    continue;

                enemyList[i].GetComponent<EnemyBehaviour>().Damage(m_BulletData.stats.stats.damage);

                Vector3 pos = new Vector3(enemyList[i].transform.position.x, enemyList[i].transform.position.y+1f, enemyList[i].transform.position.z);
                Vector3 rot = transform.rotation.eulerAngles;

                rot = new Vector3(rot.x, rot.y, rot.z);
                Instantiate(VFX, pos, Quaternion.Euler(rot), bulletEffectGameObject.transform);
            }
        }
    }
}
