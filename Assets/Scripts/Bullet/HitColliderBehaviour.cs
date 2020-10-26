using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitColliderBehaviour : MonoBehaviour
{
    public BulletBehaviour parentBulletBehaviour;

    private List<int> nullIndexList;

    private void Update()
    {
        CheckNull();
    }

    private void CheckNull()
    {
        nullIndexList = new List<int>();

        for (int i = 0; i < parentBulletBehaviour.enemyList.Count; i++)
        {
            if (parentBulletBehaviour.enemyList[i] == null)
                nullIndexList.Add(i);
        }

        for (int i = nullIndexList.Count - 1; i >= 0; i--)
        {
            parentBulletBehaviour.enemyList.RemoveAt(i);
        }
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
