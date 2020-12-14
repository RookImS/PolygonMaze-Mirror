using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexagonBehaviour : TowerBehaviour
{
    public GameObject hexagonFirstChargeEffect;
    public GameObject hexagonChargeEffect;
    public GameObject hexagonTrailEffect;

    private GameObject m_hexagonTrailEffect;

    private Coroutine coroutine;
    private Transform EnemyList;
    private AudioSource myAudioSource;

    protected override void Update()
    {
        if (target == null)         // 처음 쏠때
        {
            ((HexagonData)m_TowerData).ReloadBullet();

            if (hexagonFirstChargeEffect.activeSelf || hexagonChargeEffect.activeSelf)
            {
                if(coroutine != null)
                    StopCoroutine(coroutine);

                hexagonFirstChargeEffect.SetActive(false);
            }

            SetTarget();

            if (target != null)
            {
                muzzle.LookAt(target.transform);

                coroutine = StartCoroutine(ShootTrailDelay(2.8f));
                SoundManager.instance.PlaySound(myAudioSource, SoundManager.TowerSoundSpecific.HEXAGON, "Hexagon_First_Charge");
                hexagonFirstChargeEffect.SetActive(true);
            }

            fireCountDown = m_TowerData.Stats.stats.aheadDelay;
        }

        if (target != null)          // 실제로 countdown 계산 및 처음 쏘는 것이 아닐때
        {
            muzzle.LookAt(target.transform);
            ((HexagonData)m_TowerData).LocateBullet(target);

            if (fireCountDown <= 0f)
            {
                Attack();
                fireCountDown = 1f / m_TowerData.Stats.stats.attackRate;

                coroutine = StartCoroutine(ShootTrailDelay(0.8f));

                hexagonChargeEffect.SetActive(false);               
                hexagonChargeEffect.SetActive(true);

                SoundManager.instance.PlaySound(myAudioSource, SoundManager.TowerSoundSpecific.HEXAGON, "Hexagon_Charge");
            }
            if (fireCountDown > 0f)
                fireCountDown -= Time.deltaTime;
        }
    }

    protected override void Init()
    {
        target = null;

        m_TowerData = GetComponent<TowerData>();

        if (SoundManager.Instance != null)
            myAudioSource = SoundManager.Instance.AllocateAudio();

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

    private IEnumerator ShootTrailDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (target != null)
        {
            m_hexagonTrailEffect = Instantiate(hexagonTrailEffect, muzzle);
            m_hexagonTrailEffect.transform.position += new Vector3(0f, 1f, 0f);

            float distance = Vector3.Distance(muzzle.position, target.transform.position);
            var main = m_hexagonTrailEffect.GetComponent<ParticleSystem>().main;
            main.startLifetime = distance / 50f;
        }

        if(SoundManager.Instance != null)
            SoundManager.Instance.PlaySound(SoundManager.TowerSoundSpecific.HEXAGON, "Hexagon_Shoot");
    }

    private void OnDestroy()
    {
        if (SoundManager.Instance != null)
            SoundManager.Instance.FreeAudio(myAudioSource);
    }
}