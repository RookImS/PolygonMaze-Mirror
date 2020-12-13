using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoUI : MonoBehaviour
{
    public GameObject info;
    public GameObject range;
    public GameObject rangeInfinity;
    private GameObject selectedTower;

    public void EnableInfo(GameObject tower)
    {
        DisableInfo();
        TowerData data = tower.GetComponent<TowerData>();

        if (data is HexagonData)
        {
            range.SetActive(false);
            rangeInfinity.SetActive(true);
            rangeInfinity.transform.localScale = new Vector3(data.Stats.stats.recogRange * 2, data.Stats.stats.recogRange * 2, 1);
        }
        else
        {
            range.SetActive(true);
            rangeInfinity.SetActive(false);
            range.transform.localScale = new Vector3(data.Stats.stats.recogRange * 2, data.Stats.stats.recogRange * 2, 1);
        }

        info.SetActive(true);
        info.transform.position = new Vector3(tower.transform.position.x, 1.1f, tower.transform.position.z);

        selectedTower = tower;
        tower.GetComponent<TowerBehaviour>().isClicked = true;
    }

    public void DisableInfo()
    {
        if (selectedTower == null)
            return;

        info.SetActive(false);

        selectedTower.GetComponent<TowerBehaviour>().isClicked = false;
    }
    
}
