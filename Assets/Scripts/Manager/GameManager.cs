using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoSingleton<GameManager>
{
    [System.Serializable]
    public class AllDeckInfo
    {
        [System.Serializable]
        public class DeckInfo
        {
            public List<string> deckIDs;
        }

        public List<DeckInfo> deckInfos;

    }
    public Stack<string> sceneStack = new Stack<string>();  //BackKey 기능을 위해 씬 Buildindex를 저장하는 스택 

    public SkillsScriptableObject skillResource;
    public List<GameObject> skills;

    public List<List<GameObject>> deckList;
    public AllDeckInfo allDeckInfo;
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
        LoadDeckInfos();
    }

    private void LoadResource()
    {
        skillResource = (SkillsScriptableObject)Resources.Load("SkillList", typeof(SkillsScriptableObject));
        skills = skillResource.skillList; 
    }

    private void LoadDeckInfos()
    {
        MakeEmptyDeck();

        for (int i = 0; i < 3; i++)
        {
            string path = string.Format("Assets/UserData/DeckData/Deck{0}.json", i);
            System.IO.FileInfo file = new System.IO.FileInfo(path);

            try
            {
                if (File.Exists(path))
                {
                    string jsonData = File.ReadAllText(path);
                    this.allDeckInfo = JsonUtility.FromJson<AllDeckInfo>(jsonData);
                }
                else
                {
                    return;
                }
            }
            catch (System.ArgumentException e1)
            {
                Debug.Log(e1.Message);

            }
            catch (System.Exception e2)
            {
                Debug.Log(e2.Message);
            }

            for(int j = 0; j < 3; j++)
            {
                for(int k = 0; k < skills.Count; k++)
                {
                    if (skills[k].GetComponent<Skill>().id
                        == this.allDeckInfo.deckInfos[i].deckIDs[j])
                        deckList[i][j] = skills[k];
                    else if ("space"
                        == this.allDeckInfo.deckInfos[i].deckIDs[j])
                        deckList[i][j] = null;
                }
            }
        }
    }

    private bool CheckSkillNull(List<GameObject> deck)
    {
        for (int i = 0; i < deck.Count; i++)
            if (deck[i] == null)
                return true;
        return false;
    }

    public void SaveDeckInfos(int order)
    {
        if (CheckSkillNull(deckList[order]))
        {
            for (int i = 0; i < 3; i++)
            {
                allDeckInfo.deckInfos[order].deckIDs[i] = "space";
            }
        }
        else
        {
            for (int i = 0; i < 3; i++)
            {
                allDeckInfo.deckInfos[order].deckIDs[i] = deckList[order][i].GetComponent<Skill>().id;
            }
        }

        string jsonData = JsonUtility.ToJson(allDeckInfo);
        string path = string.Format("Assets/UserData/DeckData/Deck{0}.json", order);

        System.IO.FileInfo file = new System.IO.FileInfo(path);
        file.Directory.Create();

        try
        {
            File.WriteAllText(file.FullName, jsonData);
        }
        catch (System.ArgumentException e1)
        {
            Debug.Log(e1.Message);
            // stage name 재입력 event
        }
        catch (System.Exception e2)
        {
            Debug.Log(e2.Message);
            // IOException or UnauthorizedAccessException
        }
    }

    private void MakeEmptyDeck()
    {
        deckList = new List<List<GameObject>>();
        allDeckInfo = new AllDeckInfo();
        allDeckInfo.deckInfos = new List<AllDeckInfo.DeckInfo>();
        for (int i = 0; i < 3; i++)
        {
            deckList.Add(new List<GameObject>());
            allDeckInfo.deckInfos.Add(new AllDeckInfo.DeckInfo());
            allDeckInfo.deckInfos[i].deckIDs = new List<string>();
            for (int j = 0; j < 3; j++)
            {
                deckList[i].Add(null);
                allDeckInfo.deckInfos[i].deckIDs.Add("space");
            }
        }
    }
    
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);

        if (sceneName == "StageScene")
            SoundManager.instance.PlayBGM("InGame_BGM");
        else
            SoundManager.instance.PlayBGM("OutGame_BGM");

    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) //씬이 로드되면 로드된 씬의 buildindex를 스택에 저장.
    {
        sceneStack.Push(scene.name);
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