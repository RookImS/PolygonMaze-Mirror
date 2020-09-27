using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitColliderBehaviour : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            transform.parent.gameObject.GetComponent<BulletBehaviour>().EnemyList.Add(other.gameObject);
        }
    }
}
