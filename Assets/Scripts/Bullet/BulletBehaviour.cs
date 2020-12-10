using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    protected BulletData m_BulletData;
    public List<GameObject> enemyList;
    public GameObject hitCollider;

    public GameObject VFX;
    protected void Awake()
    {
        m_BulletData = GetComponent<BulletData>();
        enemyList = new List<GameObject>();
    }

    public virtual void Init(TowerStatSystem t_stat)
    {
        m_BulletData.Init(t_stat);
        hitCollider.GetComponent<SphereCollider>().radius = m_BulletData.stats.stats.splashRange;
    }

    protected virtual void Attack()
    {
        
    }

}
