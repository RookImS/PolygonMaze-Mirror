using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    BulletData m_BulletData;
    public List<GameObject> EnemyList;

    private void Awake()
    {
        m_BulletData = GetComponent<BulletData>();
    }
    private void Start()
    {
        EnemyList = new List<GameObject>();
        CheckRange();
    }

    private void Update()
    {
        Act();

        if(EnemyList.Count > 0)
        {
            foreach(GameObject enemy in EnemyList)
            {
                enemy.GetComponent<EnemyBehaviour>().Damage(m_BulletData.stats.stats.damage);
            }
            Destroy(this.gameObject);
        }
    }

    public void CheckRange()
    {
        StartCoroutine(m_BulletData.CheckLiving());
    }

    public void Act()
    {
        m_BulletData.Act();
    }
    
    public void Attack()
    {
        m_BulletData.Attack();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (TagManager.Instance.isEnemyTag(other.gameObject.tag))
        {
            Attack();            
        }
    }
}
