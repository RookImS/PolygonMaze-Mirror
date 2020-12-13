using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpUI : MonoBehaviour
{
    public List<HpBar> hpBarList;

    private void Awake()
    {
        hpBarList = new List<HpBar>();
    }

    private void LateUpdate()
    {
        for (int i = 0; i < hpBarList.Count; i++)
        {
            Vector3 ownerPos = hpBarList[i].owner.transform.position;
            hpBarList[i].m_hpBar.transform.position = new Vector3(ownerPos.x, ownerPos.y + 0.6f, ownerPos.z + 0.5f);

            if(hpBarList[i].needChange)
            {
                hpBarList[i].changeHp.value = Mathf.Lerp(hpBarList[i].changeHp.value, hpBarList[i].currentHp.value, Time.deltaTime * 5);
            }
        }
    }
}
