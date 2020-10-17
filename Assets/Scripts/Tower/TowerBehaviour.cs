using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBehaviour : MonoBehaviour
{
    TowerData m_TowerData;

    public Transform target;
    public List<GameObject> targetList;

    public Transform muzzle;
    float fireCountDown;

    void Awake()
    {
        Init();
    }
    void Start()
    {
        fireCountDown = 0f;
    }
    void Update()
    {
        if (targetList.Count > 0)
        {
            if (target == null)
            {
                SetTarget();
            }
        }
        if (target == null)
        {
            if(fireCountDown != 0f)
                fireCountDown -= Time.deltaTime;
            return;
        }

        SetDirection();

        if(fireCountDown <= 0f)
        {
            Invoke("Attack", m_TowerData.Stats.stats.aheadDelay);
            fireCountDown = 1f / m_TowerData.Stats.stats.attackRate;
        }
        fireCountDown -= Time.deltaTime;
        Debug.Log("cooldown : " + fireCountDown);
    }

    public void Init()
    {
        m_TowerData = GetComponent<TowerData>();
        m_TowerData.Init();
        targetList = new List<GameObject>();
        target = null;

        muzzle.GetComponent<SphereCollider>().radius = m_TowerData.Stats.stats.recogRange;
    }

    public void SetNeighbor(GameObject obj)
    {
        m_TowerData.neighbor.Add(obj);
    }

    public void SetTarget()
    {
        GameObject nearestEnemy = null;
        float shortestDistance = Mathf.Infinity;
        
        foreach (GameObject Enemy in targetList)
        {
            if (Enemy == null)
                continue;

            float distanceToEnemy = Vector3.Distance(transform.position, Enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = Enemy;
            }
        }
        if (nearestEnemy != null)
        {
            target = nearestEnemy.transform;
        }
        else
        {
            target = null;
        }
    }

    public void SetDirection()
    {
        muzzle.LookAt(target.position);
    }

    public void Attack()
    {
        m_TowerData.Shoot(muzzle);
    }

    public void DeleteTarget()
    {
        target = null;
    }


}