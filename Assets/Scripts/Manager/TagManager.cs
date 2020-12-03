using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagManager : MonoSingleton<TagManager>
{
    List<string> buildingObjectTagList;
    List<string> enemyTagList;

    private void Awake()
    {
        buildingObjectTagList = new List<string>();
        enemyTagList = new List<string>();
        Init();
    }

    public void Init()
    {
        buildingObjectTagList.Add("Tower");
        buildingObjectTagList.Add("Obstacle");
        buildingObjectTagList.Add("Wall");
        buildingObjectTagList.Add("Spawner");
        buildingObjectTagList.Add("Destination");

        enemyTagList.Add("Enemy");
    }
    public bool isNotDeployableTag(GameObject obj)
    {
        for (int i = 0; i < buildingObjectTagList.Count; i++)
        {
            if (obj.CompareTag(buildingObjectTagList[i]))
                return true;
        }

        for (int i = 0; i < enemyTagList.Count; i++)
        {
            if (obj.CompareTag(enemyTagList[i]))
                return true;
        }

        return false;
    }

    public bool isBuildingObjectTag(GameObject obj)
    {
        for (int i = 0; i < buildingObjectTagList.Count; i++)
        {
            if (obj.CompareTag(buildingObjectTagList[i]))
                return true;
        }

        return false;
    }
    public bool isEnemyTag(GameObject obj)
    {
        for (int i = 0; i < enemyTagList.Count; i++)
        {
            if (obj.CompareTag(enemyTagList[i]))
                return true;
        }

        return false;
    }
}
