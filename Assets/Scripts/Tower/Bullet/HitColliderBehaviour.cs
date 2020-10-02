using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitColliderBehaviour : MonoBehaviour
{
    public GameObject parentBullet;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            parentBullet.GetComponent<BulletBehaviour>().EnemyList.Add(other.gameObject);
        }
    }
}
