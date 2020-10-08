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
    public bool isNotDeployableTag(string tag)
    {
        foreach (string checkTag in buildingObjectTagList)
        {
            if (checkTag.Equals(tag))
                return true;
        }

        foreach (string checkTag in enemyTagList)
        {
            if (checkTag.Equals(tag))
                return true;
        }

        return false;
    }

    public bool isBuildingObjectTag(string tag)
    {
        foreach (string checkTag in buildingObjectTagList)
        {
            if (checkTag.Equals(tag))
                return true;
        }

        return false;
    }
    public bool isEnemyTag(string tag)
    {
        foreach (string checkTag in enemyTagList)
        {
            if (checkTag.Equals(tag))
                return true;
        }

        return false;
    }
}
