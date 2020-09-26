using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class EventManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static EventManager instance;
    public UnityEvent OnDeploy;
    public DeployBehavior deployBehavior;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(obj: this);
        }
    }

    void Start()
    {
        //DeployTower += PlayerControl.Instance.UseCost();
    }
    public event Action DeployTower;


    public void OnDeployTower()
    {
        DeployTower?.Invoke();
    }

    private void Deploy()
    {

        OnDeploy.Invoke();
    }
    

}
