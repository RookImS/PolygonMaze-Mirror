using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class SideColliderBehavior : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Tower") || other.gameObject.CompareTag("Neutral"))
        {
            if (!this.transform.parent.Equals(other.transform.parent))
            {
                this.transform.parent.GetComponent<TowerBehaviour>().setNeighbor(other.gameObject.transform.parent.gameObject);     // 이웃한 프리팹 본체를 등록한다.
                this.gameObject.GetComponent<BoxCollider>().enabled = false;
            }
        }
    }
}
