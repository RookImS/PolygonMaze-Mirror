using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBehaviour : MonoBehaviour
{
    TowerData m_TowerData;

    public Transform target;
    List<GameObject> targetList;

    public Transform firePoint;
    float fireCountDown;

    void Start()
    {
        Init();
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
            return;

        SetDirection();

        if(fireCountDown <= 0f)
        {
            Invoke("Attack", m_TowerData.Stats.stats.aheadDelay);
            fireCountDown = 1f / m_TowerData.Stats.stats.attackRate;
        }
        fireCountDown -=Time.deltaTime;
    }


    public void Init()
    {
        m_TowerData = GetComponent<TowerData>();
        m_TowerData.Init();
        targetList = new List<GameObject>();
        target = null;

        GetComponent<SphereCollider>().radius = m_TowerData.Stats.stats.recogRange;
    }


    public void setNeighbor(GameObject obj)
    {
        m_TowerData.neighbor.Add(obj);
    }

    public void SetTarget()
    {
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;
        foreach (GameObject Enemy in targetList)
        {
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
        firePoint.LookAt(target.position);
    }

    public void Attack()
    {
        m_TowerData.Shoot(firePoint);
    }

    void DeleteTarget()
    {
        target = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            targetList.Add(other.gameObject);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            targetList.Remove(other.gameObject);
            if (target.gameObject == other.gameObject)
            {
                DeleteTarget();
                SetTarget();
            }

        }
    }
}