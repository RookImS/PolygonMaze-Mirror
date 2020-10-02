using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class SideColliderBehavior : MonoBehaviour
{
    public GameObject parentObject;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Tower") || other.gameObject.CompareTag("Neutral") 
            ||other.gameObject.CompareTag("Wall") || other.gameObject.CompareTag("WallCornerSide1") || other.gameObject.CompareTag("WallCornerSide2") || other.gameObject.CompareTag("Spawner") || other.gameObject.CompareTag("Destination"))
        {
            if (!parentObject.Equals(other.gameObject))
            {
                GetComponent<Collider>().enabled = false;
            }
        }
    }
}
