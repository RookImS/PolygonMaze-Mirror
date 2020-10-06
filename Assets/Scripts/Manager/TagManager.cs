using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagManager : MonoSingleton<TagManager>
{
    List<string> notDeployableTagList;
    List<string> enemyTagList;

    private void Awake()
    {
        notDeployableTagList = new List<string>();
        enemyTagList = new List<string>();
    }

    public void Init()
    {
        notDeployableTagList.Add("Tower");
        notDeployableTagList.Add("Enemy");
        notDeployableTagList.Add("Obstacle");
        notDeployableTagList.Add("Wall");
        notDeployableTagList.Add("Spawner");
        notDeployableTagList.Add("Destination");

        enemyTagList.Add("Enemy");
    }
    public bool isNotDeployableTag(string tag)
    {
        foreach (string checkTag in notDeployableTagList)
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
