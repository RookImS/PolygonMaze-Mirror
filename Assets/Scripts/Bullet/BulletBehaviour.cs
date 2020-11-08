using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    protected BulletData m_BulletData;
    public List<GameObject> enemyList;
    public GameObject hitCollider;

    protected void Awake()
    {
        m_BulletData = GetComponent<BulletData>();
        enemyList = new List<GameObject>();
    }

    public virtual void Init(TowerStatSystem t_stat)
    {

    }

    public virtual void Init(TowerStatSystem t_stat, int ticRate, float attackDuration)
    {

    }

    protected virtual void Attack()
    {
        
    }

}
