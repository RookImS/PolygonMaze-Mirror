using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    public GameObject hpBar;

    [HideInInspector] public GameObject hpUI;
    [HideInInspector] public GameObject owner;
    [HideInInspector] public EnemyData ownerData;
    [HideInInspector] public GameObject m_hpBar;
    [HideInInspector] public Slider currentHp;
    [HideInInspector] public Slider changeHp;
    [HideInInspector] public bool needChange;

    public void Init()
    {
        owner = this.gameObject;
        ownerData = this.gameObject.GetComponent<EnemyData>();
        hpUI = GameObject.Find("HpUI");

        m_hpBar = Instantiate(hpBar, hpUI.transform);

        currentHp = m_hpBar.transform.Find("currentHp").gameObject.GetComponent<Slider>();
        currentHp.maxValue = ownerData.Stats.stats.hp;
        currentHp.value = currentHp.maxValue;

        changeHp = currentHp.transform.Find("changeHp").gameObject.GetComponent<Slider>();
        changeHp.maxValue = ownerData.Stats.stats.hp;
        changeHp.value = currentHp.maxValue;

        hpUI.GetComponent<HpUI>().hpBarList.Add(this);

        m_hpBar.SetActive(false);
    }

    public void ApplyHpBar()
    {
        currentHp.maxValue = (float)ownerData.Stats.stats.hp;
        changeHp.maxValue = currentHp.maxValue;
        currentHp.value = (float)ownerData.Stats.currentHp;
        Invoke("CheckNeedChange", 0.5f);
    }

    public bool CheckNeedChange()
    {
        if (currentHp.value != changeHp.value)
            needChange = true;
        else
            needChange = false;

        return needChange;
    }

    public void DestroyHpBar()
    {
        hpUI.GetComponent<HpUI>().hpBarList.Remove(this);
        Destroy(m_hpBar);
    }
}
