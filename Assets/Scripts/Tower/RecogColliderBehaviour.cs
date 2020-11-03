using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecogColliderBehaviour : MonoBehaviour
{
    public TowerBehaviour parentTowerBehaviour;

    private List<int> nullIndexList;

    private void Update()
    {
        CheckNull();
    }

    private void CheckNull()
    {
        nullIndexList = new List<int>();

        for (int i = 0; i < parentTowerBehaviour.targetList.Count; i++)
        {
            if (parentTowerBehaviour.targetList[i] == null)
                nullIndexList.Add(i);
        }

        for(int i = nullIndexList.Count - 1; i >= 0; i--)
        {
            parentTowerBehaviour.targetList.RemoveAt(i);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (TagManager.Instance.isEnemyTag(other.gameObject.tag))
        {
            parentTowerBehaviour.targetList.Add(other.gameObject);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (TagManager.Instance.isEnemyTag(other.gameObject.tag))
        {
            parentTowerBehaviour.targetList.Remove(other.gameObject);
            if (parentTowerBehaviour.target == other.gameObject)
            {
                parentTowerBehaviour.DeleteTarget();
            }
        }
    }
}