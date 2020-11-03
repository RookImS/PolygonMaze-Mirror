using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBehaviour : MonoBehaviour
{
    public Transform muzzle;

    [HideInInspector] public GameObject target;
    [HideInInspector] public List<GameObject> targetList;

    private TowerData m_TowerData;
    private float fireCountDown;

    private void Awake()
    {
        Init();
    }
    private void Update()
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
            if (fireCountDown != 0f)
                fireCountDown -= Time.deltaTime;
            return;
        }

        SetDirection();

        if (fireCountDown <= 0f)
        {
            Invoke("Attack", m_TowerData.Stats.stats.aheadDelay);
            fireCountDown = 1f / m_TowerData.Stats.stats.attackRate;
        }
        fireCountDown -= Time.deltaTime;
    }

    private void Init()
    {
        targetList = new List<GameObject>();
        target = null;

        m_TowerData = GetComponent<TowerData>();
        m_TowerData.Init();
        fireCountDown = 0f;

        muzzle.GetComponent<SphereCollider>().radius = m_TowerData.Stats.stats.recogRange;
    }

    private void SetDirection()
    {
        muzzle.LookAt(target.transform.position);
    }
    private void SetTarget()
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
            target = nearestEnemy;
        }
        else
        {
            target = null;
        }
    }

    private void Attack()
    {
        m_TowerData.Shoot(muzzle);
    }

    public void SetNeighbor(GameObject obj)
    {
        m_TowerData.neighbor.Add(obj);
    }

    public void DeleteTarget()
    {
        target = null;
    }
}