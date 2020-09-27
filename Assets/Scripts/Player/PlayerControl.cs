using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class PlayerControl : MonoBehaviour
{
    private static PlayerControl instance;
    public static PlayerData playerData;

    bool isGameOver;

    public static PlayerControl Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = FindObjectOfType<PlayerControl>();
                if(obj != null)
                {
                    instance = obj;
                }
                else
                {
                    GameObject gobj = new GameObject("test");
                    var temp = gobj.AddComponent<PlayerControl>();
                    playerData = gobj.AddComponent<PlayerData>();
                    instance = temp;
                }

            }
            return instance;
        }
        private set
        {
            instance = value;
        }
    }
    private void Awake()
    {
        var objs = FindObjectsOfType<PlayerControl>();
        if(objs.Length != 1)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        //GameManager.Instance.DeployTower += UseCost
    }
    public void Init()
    {
        Debug.Log("Init");
        playerData.Init();
    }

    public void SetPlayer(int cost, int life)
    {
        playerData.SetPlayer(cost, life);
    }

    public void GainCost(int reward)
    {
        playerData.ChangeCost(reward);
    }

    public bool UseCost(int cost)
    {
        return playerData.ChangeCost(-cost);
    }

    public void Damage(int life)
    {
        playerData.ChangeLife(-life);
    }

    public void Death()
    {
        playerData.Death();
    }

}

