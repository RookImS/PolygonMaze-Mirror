using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class SideColliderBehaviour : MonoBehaviour
{
    public GameObject parentObject;

    private void OnTriggerEnter(Collider other)
    {
        if (TagManager.Instance.isBuildingObjectTag(other.gameObject))
        {
            if (!parentObject.Equals(other.gameObject))
            {
                GetComponent<Collider>().enabled = false;
            }
        }
    }
}