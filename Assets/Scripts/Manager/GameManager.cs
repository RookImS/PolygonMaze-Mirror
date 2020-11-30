using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoSingleton<GameManager>
{
    public Stack<int> sceneStack = new Stack<int>();  //BackKey 기능을 위해 씬 Buildindex를 저장하는 스택 

    public SkillsScriptableObject skillResource;
    public List<GameObject> skills;

    public List<List<GameObject>> deckList = new List<List<GameObject>>();
    public List<GameObject> deck1 = new List<GameObject>();
    public List<GameObject> deck2 = new List<GameObject>();
    public List<GameObject> deck3 = new List<GameObject>();
    public List<GameObject> currentDeck;

    public bool isStart;
    private int loadStageChapter;
    private int loadStageLevel;

    private void Awake()
    {
        Init();
    }

    public void Init()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        loadStageChapter = 0;
        loadStageLevel = 0;

        LoadResource();
    }
    private void LoadResource()
    {
        skillResource = (SkillsScriptableObject)Resources.Load("SkillList", typeof(SkillsScriptableObject));
        skills = skillResource.skillList;

        Deckmake();
    }
    private void Deckmake()
    {
        deck1 = new List<GameObject>();
        deck2 = new List<GameObject>();
        deck3 = new List<GameObject>();

        deckList.Add(deck1);
        deckList.Add(deck2);
        deckList.Add(deck3);

        for (int i = 0; i < deckList.Count; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                deckList[i].Add(null);
            }
        }

       
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode) //씬이 로드되면 로드된 씬의 buildindex를 스택에 저장.
    {
        sceneStack.Push(scene.buildIndex);
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

    // ---------------------- INGAME ----------------------

    [HideInInspector] public GameObject inGameUI;
    [HideInInspector] public bool isGameOver;
    [HideInInspector] public bool isStageClear;
    public void InitIngameSetting()
    {
        isGameOver = false;
        isStageClear = false;
        inGameUI = GameObject.Find("InGameUI");
    }

    public void ReturnToMain()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void RestartGame()
    {
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

    private void LoadStage()
    {
        inGameUI = GameObject.Find("InGameUI");
    }

    public void GameOver()
    {
        isGameOver = true;
        PanelSystem panelSys = inGameUI.transform.Find("IngamePanel").gameObject.GetComponent<PanelSystem>();
        panelSys.SetPanel(panelSys.gameOverPanel);
    }

    public void StageClear()
    {
        isStageClear = true;
        PanelSystem panelSys = inGameUI.transform.Find("IngamePanel").gameObject.GetComponent<PanelSystem>();
        panelSys.SetPanel(panelSys.stageClearPanel);
    }
}