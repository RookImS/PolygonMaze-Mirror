using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public static PlayerData playerdata;
    public static TowerData towerdata;
    public static EnemyData enemydata;
    public static DeployBehavior deploybehavior;
    public static TowerBehaviour towerbehaviour;
    public static UISystem uisystem;
    bool isGameOver;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = FindObjectOfType<GameManager>();
                if (obj != null)
                {
                    instance = obj;
                }
                else
                {
                    GameObject gobj = new GameObject("GameManager");
                    var temp = gobj.AddComponent<GameManager>();

                    //playerdata = gobj.AddComponent<PlayerData>();
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
        var objs = FindObjectsOfType<GameManager>();
        if (objs.Length != 1)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {

        GameStart += PlayerControl.Instance.Init;

    }

    public event Action GameStart;
    public event Action DeployTower;
    public event Action EnemyEscape;
    public event Action Exit;

    public void OnDeployTower()
    {
        DeployTower?.Invoke();
    }

    public void FailDeployTower()
    {

    }
    // Start is called before the first frame update

    public void Init()
    {

    }

    public delegate void PrintDelegate(string message);

    public event PrintDelegate printAllEvent;

    // Update is called once per frame

    void Update()
    {
        
    }

}
