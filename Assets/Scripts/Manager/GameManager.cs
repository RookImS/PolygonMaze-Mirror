using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public static PlayerData playerdata;
    public static TowerData towerdata;
    public static EnemyData enemydata;
    //public static DeployBehavior deploybehavior;
    public static TowerBehaviour towerbehaviour;
    //public static UISystem uisystem;

    public GameState currentGameState;
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
        currentGameState = GameState.menu;

        GameStart += PlayerControl.Instance.Init;
        GameStart += StartGame;

        DeployTower += TimeRestore;
        Debug.Log("start");

    }
    public static Queue<int> queue = new Queue<int>();  //BackKey 기능을 위해 씬 Buildindex를 저장하는 큐

    public enum GameState
    {
        menu,
        inGame,
        gameOver
    }

    void SetGameState(GameState newGameState)  //현재 게임플레이 상태 지정
    {
        if (newGameState == GameState.menu)
        {
        }
        else if (newGameState == GameState.inGame)
        {
        }
        else if (newGameState == GameState.gameOver)
        {
        }
        currentGameState = newGameState;
    }

    public event Action GameStart;
    public event Action DeployTower;
    public event Action EnemyEscape;
    public event Action Exit;
    public event Action PlayerDeath;
    public event Action EnemyDeath;

    private void OnEnable() //GameManager 활성화시 sceneLoaded 이벤트에 OnSceneLoaded 함수 추가
    {
        Debug.Log("활성화");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()    //GameManager 비활성화시 sceneLoaded 이벤트에서 OnSceneLoaded 함수 제거(비활성화 될 일이 있나?)
    {
        Debug.Log("비활성화");
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void StartGame()
    {
        SetGameState(GameState.inGame);
    }

    public void OnDeployTower()
    {
        DeployTower?.Invoke();
    }


    void OnSceneLoaded(Scene scene, LoadSceneMode mode) //씬이 로드되면 로드된 씬의 buildindex를 큐에 저장.
    {
        Debug.Log("씬이동");
        queue.Enqueue(scene.buildIndex);
        Debug.Log(scene.buildIndex);
        Debug.Log("OnSceneLoaded : " + scene.name);
    }

    public void OnFailDeployTower()
    {

    }

    public void OnEnemyDeath()
    {
        EnemyDeath?.Invoke();
    }

    public void OnPlayerDeath()
    {
        PlayerDeath?.Invoke();
    }

    public void SlowTime()
    {
        Time.timeScale = 0.5f;
    }

    public void TimeRestore()
    {
        Time.timeScale = 1f;
    }

    public void TimeStop()
    {
        Time.timeScale = 0f;
    }

    // Update is called once per frame

    void Update()
    {

    }

    void test()
    {
        byte[] byteTexture = System.IO.File.ReadAllBytes(System.IO.Path.Combine(Application.streamingAssetsPath, "Star"));
        if (byteTexture.Length > 0)
        {
            Texture2D texture = new Texture2D(0, 0);
            texture.LoadImage(byteTexture);
            //Sprite sprite;
            gameObject.GetComponent<Image>().sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            // = System.IO.File.ReadAllBytes(Path);
        }
    }

}
