using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class SideColliderBehavior : MonoBehaviour
{
    public GameObject parentObject;

    private void OnTriggerEnter(Collider other)
    {
        if (TagManager.Instance.isNotDeployableTag(other.gameObject.tag))
        {
            if (!parentObject.Equals(other.gameObject))
            {
                GetComponent<Collider>().enabled = false;
            }
        }
    }
}
