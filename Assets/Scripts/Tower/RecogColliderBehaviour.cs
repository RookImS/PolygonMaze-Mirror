using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecogColliderBehaviour : MonoBehaviour
{
    public TowerBehaviour parentTowerBehaviour;

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
