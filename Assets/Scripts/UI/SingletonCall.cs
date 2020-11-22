using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonCall : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        lnit();
    }
    void lnit()
    {
        GameManager.Instance.Init();
    }

    // Update is called once per frame
}
