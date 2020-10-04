using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckOverlap : MonoBehaviour
{
    public bool isOverlapped = true;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Tower"))
        {
            Debug.Log("tower overlap");
            isOverlapped = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Tower"))
        {
            Debug.Log("tower not overlap");
            isOverlapped = false;
        }
    }

}
