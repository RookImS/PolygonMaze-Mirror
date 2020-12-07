using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoUI : MonoBehaviour
{
    public GameObject info;
    public GameObject range;

    private GameObject selectedTower;

    public void EnableInfo(GameObject tower)
    {
        DisableInfo();
        TowerData data = tower.GetComponent<TowerData>();
        info.SetActive(true);
        info.transform.position = new Vector3(tower.transform.position.x, 1.1f, tower.transform.position.z);
        range.transform.localScale = new Vector3(data.Stats.stats.recogRange * 2, data.Stats.stats.recogRange * 2, 1);

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
