using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitColliderBehaviour : MonoBehaviour
{
    public BulletBehaviour parentBulletBehaviour;
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
        for (int i = 0; i < parentBulletBehaviour.enemyList.Count; i++)
        {
            if (parentBulletBehaviour.enemyList[i] == null)
                nullIndexList.Add(i);
        }

        foreach (int i in nullIndexList)
        {
            parentBulletBehaviour.enemyList.RemoveAt(i);
        }

        nullIndexList.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(TagManager.Instance.isEnemyTag(other.gameObject.tag))
        {
            parentBulletBehaviour.enemyList.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(TagManager.Instance.isEnemyTag(other.gameObject.tag))
        {
            parentBulletBehaviour.enemyList.Remove(other.gameObject);
        }
    }
}
