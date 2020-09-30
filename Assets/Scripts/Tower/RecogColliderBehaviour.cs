using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecogColliderBehaviour : MonoBehaviour
{
    public GameObject tower;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            tower.GetComponent<TowerBehaviour>().targetList.Add(other.gameObject);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            tower.GetComponent<TowerBehaviour>().targetList.Remove(other.gameObject);
            if (tower.GetComponent<TowerBehaviour>().target.gameObject == other.gameObject)
            {
                tower.GetComponent<TowerBehaviour>().DeleteTarget();
                tower.GetComponent<TowerBehaviour>().SetTarget();
            }

        }
    }
}
