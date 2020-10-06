using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitColliderBehaviour : MonoBehaviour
{
    public GameObject parentBullet;

    private void OnTriggerEnter(Collider other)
    {
        if(TagManager.Instance.isEnemyTag(other.gameObject.tag))
        {
            parentBullet.GetComponent<BulletBehaviour>().EnemyList.Add(other.gameObject);
        }
    }
}
