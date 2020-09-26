using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeployTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        //Debug.Log("들어옴");
        PlayerControl.Instance.Init();   
    }
}
