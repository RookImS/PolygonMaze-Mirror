using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexagonBehaviour : TowerBehaviour
{
    public Transform EnemyList;

    protected override void Update()
    {
        if (target == null)         // 처음 쏠때
        {
            ((HexagonData)m_TowerData).ReloadBullet();

            SetTarget();
            fireCountDown = m_TowerData.Stats.stats.aheadDelay;
        }

        if (target != null)          // 실제로 countdown 계산 및 처음 쏘는 것이 아닐때
        {
            ((HexagonData)m_TowerData).LocateBullet(target);

            if (fireCountDown <= 0f)
            {
                Attack();
                fireCountDown = 1f / m_TowerData.Stats.stats.attackRate;
            }
            if (fireCountDown > 0f)
                fireCountDown -= Time.deltaTime;
        }
    }

    protected override void Init()
    {
        target = null;

        m_TowerData = GetComponent<TowerData>();
        m_TowerData.Init();
        fireCountDown = 0f;

        EnemyList = GameObject.Find("Waves").transform;
        isClicked = false;

        HexagonManager.instance.HexagonList.Add(this);
    }

    protected override void SetTarget()
    {
        GameObject compareEnemy = null;
        EnemyData compareEnemyData = null;
        int highestDef = -1;
        bool isDisableWave = false;
        int totalEnemyNumber = 0;

        for (int waveNo = 0; waveNo < EnemyList.childCount; waveNo++)    // 모든 웨이브 확인
        {
            isDisableWave = false;
            for (int enemyNo = 0; enemyNo < EnemyList.GetChild(waveNo).childCount; enemyNo++)    // 웨이브의 모든 적 확인
            {
                compareEnemy = EnemyList.GetChild(waveNo).GetChild(enemyNo).gameObject;
                compareEnemyData = compareEnemy.GetComponent<EnemyData>();

                if (compareEnemy.activeSelf)      // 활성화된 적만 확인
                {
                    totalEnemyNumber++;
                    if (CheckTargetingSameEnemy(compareEnemy))
                        continue;

                    if (highestDef < compareEnemyData.Stats.stats.def)       // 방어력이 가장 센 적?
                    {
                        highestDef = compareEnemyData.Stats.stats.def;
                        target = compareEnemy;
                    }
                    else if (highestDef == compareEnemyData.Stats.stats.def)    // 방어력이 같다면
                    {
                        if (target.GetComponent<EnemyData>().Stats.currentHp <= compareEnemyData.Stats.currentHp)    // 현재 체력이 더 많은 적?
                            target = compareEnemy;
                    }
                }
                else
                {
                    if (enemyNo == 0)       // 첫번째부터 비활성화된 적인 경우 wave 시작 안함
                        isDisableWave = true;

                    break;
                }
            }
            if (isDisableWave)              // 비활성된 웨이브이면 끝
                break;
        }

        if(target == null && totalEnemyNumber < HexagonManager.instance.HexagonList.Count)
        {
            foreach(HexagonBehaviour temp in HexagonManager.instance.HexagonList)
            {
                if (temp.target != null)
                    target = temp.target;
            }
        }
    }

    private bool CheckTargetingSameEnemy(GameObject enemy)
    {
        for (int i = 0; i < HexagonManager.instance.HexagonList.Count; i++)
        {
            if (System.Object.ReferenceEquals(HexagonManager.instance.HexagonList[i].gameObject, this.gameObject))
                continue;
            else
            {
                if (System.Object.ReferenceEquals(HexagonManager.instance.HexagonList[i].target, enemy))
                    return true;
            }
        }

        return false;
    }
}