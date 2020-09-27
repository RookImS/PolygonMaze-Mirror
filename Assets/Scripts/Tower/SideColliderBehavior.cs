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
            if (!this.transform.parent.Equals(other.transform))
            {
                this.gameObject.GetComponent<Collider>().enabled = false;
            }
        }
    }
}
