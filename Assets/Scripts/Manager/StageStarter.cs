using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageStarter : MonoBehaviour
{
    private void Awake()
    {
        PlayerControl.Instance.Init();
        PlayerControl.Instance.Call();
    }
}
