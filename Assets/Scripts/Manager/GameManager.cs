using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameManager : MonoSingleton<GameManager>
{
    [HideInInspector] public GameObject inGameUI;

    public event Action gameStart;
    public event Action deployTower;
    public event Action enemyEscape;
    public event Action exit;
    public event Action gameOver;
    public event Action stageClear;
    public event Action enemyDeath;

    public GameState currentGameState;

    private int loadStageChapter;
    private int loadStageLevel;

    private void Awake()
    {
        inGameUI = GameObject.Find("InGameUI");
        Init();
    }

    void Start()
    {
        currentGameState = GameState.menu;

        //GameStart += PlayerControl.Instance.Init;
        //GameStart += StartGame;

        //DeployTower += TimeRestore;

    }
    public static Stack<int> stack = new Stack<int>();  //BackKey 기능을 위해 씬 Buildindex를 저장하는 스택

    

    public enum GameState
    {
        menu,
        inGame,
        gameOver
    }

    private void Init()
    {
        loadStageChapter = 0;
        loadStageLevel = 0;
    }

    public int GetLoadStageChapter()
    {
        return this.loadStageChapter;
    }

    public void SetLoadStageChapter(int loadStageChapter)
    {
        this.loadStageChapter = loadStageChapter;
    }

    public int GetLoadStageLevel()
    {
        return this.loadStageLevel;
    }

    public void SetLoadStageLevel(int loadStageLevel)
    {
        this.loadStageLevel = loadStageLevel;
    }

    void SetGameState (GameState newGameState)  //현재 게임플레이 상태 지정
    {
        if(newGameState == GameState.menu) { 
        } else if (newGameState == GameState.inGame) { 
        } else if (newGameState == GameState.gameOver) { 
        }
        currentGameState = newGameState;
    }
    void StartGame()
    {
        SetGameState(GameState.inGame);
    }

    private void OnEnable() //GameManager 활성화시 sceneLoaded 이벤트에 OnSceneLoaded 함수 추가
    {
        //Debug.Log("활성화");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()    //GameManager 비활성화시 sceneLoaded 이벤트에서 OnSceneLoaded 함수 제거(비활성화 될 일이 있나?)
    {
        //Debug.Log("비활성화");
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) //씬이 로드되면 로드된 씬의 buildindex를 스택에 저장.
    {
        //Debug.Log("씬이동");
        stack.Push(scene.buildIndex);
        //Debug.Log("로드된 scene buildindex: " + scene.buildIndex);
        //Debug.Log("OnSceneLoaded : " + scene.name);
    }
    public void ReturnToMain()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void RestartGame()
    {
        //PlayerControl.Instance.Init();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void SlowTime(float scale)
    {
        Time.timeScale = scale;
    }
    public void TimeRestore()
    {
        Time.timeScale = 1f;
    }
    public void TimeStop()
    {
        Time.timeScale = 0f;
    }


    private int _EnemyDeathCount = 0;
    private int _EnemyEscapeCount = 0;
    private int _DeployTowerCount = 0;

    public int EnemyDeathCount
    {
        get
        { return _EnemyDeathCount;}
    }
    public int EnemyEscapeCount
    {
        get
        { return _EnemyEscapeCount;}
    }
    public int DeployTowerCount
    {
        get{ return _DeployTowerCount;}
    }

    public void InitCounter()
    {
        _EnemyDeathCount = 0;
        _EnemyEscapeCount = 0;
        _DeployTowerCount = 0;
    }

    public void OnFailDeployTower()
    {

    }
    //public void OnDeployTower()
    //{
    //    _DeployTowerCount++;
    //    DeployTower?.Invoke();
    //}
    //public void OnEnemyDeath()
    //{
    //    _EnemyDeathCount++;
    //    EnemyDeath?.Invoke();
    //}
    //public void OnEnemyEscape()
    //{
    //    _EnemyEscapeCount++;
    //    EnemyEscape?.Invoke();
    //}

    private void LoadStage()
    {
        inGameUI = GameObject.Find("InGameUI");
    }
    public void GameOver()
    {
        PanelSystem panelSys = inGameUI.transform.Find("IngamePanel").gameObject.GetComponent<PanelSystem>();
        panelSys.SetPanel(panelSys.gameOverPanel);
    }

    public void StageClear()
    {
        PanelSystem panelSys = inGameUI.transform.Find("IngamePanel").gameObject.GetComponent<PanelSystem>();
        panelSys.SetPanel(panelSys.stageClearPanel);
    }


    //void test()
    //{
    //    byte[] byteTexture = System.IO.File.ReadAllBytes(System.IO.Path.Combine(Application.streamingAssetsPath,"Star"));
    //    if (byteTexture.Length > 0)
    //    {
    //        Texture2D texture = new Texture2D(0, 0);
    //        texture.LoadImage(byteTexture);
    //        //Sprite sprite;
    //        gameObject.GetComponent<Image>().sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    //        // = System.IO.File.ReadAllBytes(Path);
    //    }
    //}

}
