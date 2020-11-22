
using UnityEngine;

public class SideColliderBehaviour : MonoBehaviour
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