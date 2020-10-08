using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecogColliderBehaviour : MonoBehaviour
{
    public TowerBehaviour parentTowerBehaviour;
    private List<int> nullIndexList;

    private void Awake()
    {
        nullIndexList = new List<int>();
    }

    private void Update()
    {
        CheckNull();
    }

    private void CheckNull()
    {
        for (int i = 0; i < parentTowerBehaviour.targetList.Count; i++)
        {
            if (parentTowerBehaviour.targetList[i] == null)
                nullIndexList.Add(i);
        }

        foreach (int i in nullIndexList)
        {
            parentTowerBehaviour.targetList.RemoveAt(i);
        }

        nullIndexList.Clear();
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
            if (parentTowerBehaviour.target.gameObject == other.gameObject)
            {
                parentTowerBehaviour.DeleteTarget();
                parentTowerBehaviour.SetTarget();
            }
        }
    }
}
