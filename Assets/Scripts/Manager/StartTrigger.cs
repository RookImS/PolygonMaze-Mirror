using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartTrigger : MonoBehaviour
{
    private void Awake()
    {
        lnit();
    }

    private void lnit()
    {
        GameManager.Instance.isStart = true;
    }
}
