using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class SideColliderBehavior : MonoBehaviour
{
    public GameObject parentObject;

    private void OnTriggerStay(Collider other)
    {
        if (TagManager.Instance.isBuildingObjectTag(other.gameObject.tag))
        {
            if (!parentObject.Equals(other.gameObject))
            {
                GetComponent<Collider>().enabled = false;
            }
        }
    }
}
