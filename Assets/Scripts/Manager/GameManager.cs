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

        public List<DeckInfo> deck;
    }

    [System.Serializable]
    public class StageClearInfo
    {
        [System.Serializable]
        public class StageLevelInfo
        {
            [System.Serializable]
            public class StageInfo
            {
                public bool isClear;
                public bool achievement1;
                public bool achievement2;
                public bool achievement3;
            }
            public List<StageInfo> stageLevel;
        }

        public List<StageLevelInfo> stageChpater;
    }

    public Stack<string> sceneStack = new Stack<string>();  //BackKey 기능을 위해 씬 Buildindex를 저장하는 스택 

    public SkillsScriptableObject skillResource;
    public List<GameObject> skills;
    public List<List<GameObject>> deckList;
    public List<GameObject> currentDeck;

    private AllDeckInfo allDeckInfo;
    private StageClearInfo stageClearInfo;

    public bool isStart;
    private int loadStageChapter;
    private int loadStageLevel;

    private int chapterMax = 10;
    private int levelMax = 10;

    private new void Awake()
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
        LoadStageClearInfo();
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
            string path;
            if(Application.platform == RuntimePlatform.Android)
                path = Application.persistentDataPath + string.Format("/DeckData/Deck{0}.json", i);
            else
                path = string.Format("Assets/UserData/DeckData/Deck{0}.json", i);
            //string path = string.Format("Assets/UserData/DeckData/Deck{0}.json", i);
            System.IO.FileInfo file = new System.IO.FileInfo(path);

            try
            {
                if (File.Exists(path))
                {
                    string jsonData = File.ReadAllText(path);
                    this.allDeckInfo.deck[i] = JsonUtility.FromJson<AllDeckInfo.DeckInfo>(jsonData);
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

            for (int j = 0; j < 3; j++)
            {
                for (int k = 0; k < skills.Count; k++)
                {
                    if (skills[k].GetComponent<Skill>().id
                        == this.allDeckInfo.deck[i].deckIDs[j])
                        deckList[i][j] = skills[k];
                    else if ("space"
                        == this.allDeckInfo.deck[i].deckIDs[j])
                        deckList[i][j] = null;
                }
            }
        }
    }

    private void MakeEmptyDeck()
    {
        deckList = new List<List<GameObject>>();
        allDeckInfo = new AllDeckInfo();
        allDeckInfo.deck = new List<AllDeckInfo.DeckInfo>();
        for (int i = 0; i < 3; i++)
        {
            deckList.Add(new List<GameObject>());
            allDeckInfo.deck.Add(new AllDeckInfo.DeckInfo());
            allDeckInfo.deck[i].deckIDs = new List<string>();
            for (int j = 0; j < 3; j++)
            {
                deckList[i].Add(null);
                allDeckInfo.deck[i].deckIDs.Add("space");
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
                allDeckInfo.deck[order].deckIDs[i] = "space";
            }
        }
        else
        {
            for (int i = 0; i < 3; i++)
            {
                allDeckInfo.deck[order].deckIDs[i] = deckList[order][i].GetComponent<Skill>().id;
            }
        }

        string jsonData = JsonUtility.ToJson(allDeckInfo.deck[order]);
        string path;
        if (Application.platform == RuntimePlatform.Android)
            path = Application.persistentDataPath + string.Format("/DeckData/Deck{0}.json", order);
        else
            path = string.Format("Assets/UserData/DeckData/Deck{0}.json", order);
        //string path = string.Format("Assets/UserData/DeckData/Deck{0}.json", order);

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

    public void LoadStageClearInfo()
    {
        MakeEmptyStageClearInfo();
        string path;
        if (Application.platform == RuntimePlatform.Android)
            path = Application.persistentDataPath + string.Format("/StageClearData.json");
        else
            path = string.Format("Assets/UserData/StageClearData.json");
        //string path = string.Format("Assets/UserData/StageClearData.json");
        System.IO.FileInfo file = new System.IO.FileInfo(path);

        try
        {
            if (File.Exists(path))
            {
                string jsonData = File.ReadAllText(path);
                this.stageClearInfo = JsonUtility.FromJson<StageClearInfo>(jsonData);
            }
            else
            {
                
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

    }

    public void MakeEmptyStageClearInfo()
    {
        stageClearInfo = new StageClearInfo();
        stageClearInfo.stageChpater = new List<StageClearInfo.StageLevelInfo>();

        for (int i = 0; i < chapterMax; i++)
        {
            stageClearInfo.stageChpater.Add(new StageClearInfo.StageLevelInfo());
            stageClearInfo.stageChpater[i].stageLevel = new List<StageClearInfo.StageLevelInfo.StageInfo>();

            for (int j = 0; j < levelMax; j++)
            {
                stageClearInfo.stageChpater[i].stageLevel.Add(new StageClearInfo.StageLevelInfo.StageInfo());
                stageClearInfo.stageChpater[i].stageLevel[j].isClear = false;
                stageClearInfo.stageChpater[i].stageLevel[j].achievement1 = false;
                stageClearInfo.stageChpater[i].stageLevel[j].achievement2 = false;
                stageClearInfo.stageChpater[i].stageLevel[j].achievement3 = false;
            }
        }
    }

    public void SaveStageClearInfo()
    {

        string jsonData = JsonUtility.ToJson(stageClearInfo);
        string path;
        if (Application.platform == RuntimePlatform.Android)
            path = Application.persistentDataPath + string.Format("/StageClearData.json");
        else
            path = string.Format("Assets/UserData/StageClearData.json");
        //string path = string.Format("Assets/UserData/StageClearData.json");

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

    public void OnApplicationQuit()
    {
        SaveStageClearInfo();
        for (int i = 0; i < 3; i++)
            SaveDeckInfos(i);
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

        SoundManager.Instance.StopBGMPlayer();
        SoundManager.Instance.PlaySound(SoundManager.SoundSpecific.OTHERUI, "Player_Game_Over_Sound");
    }

    public void StageClear()
    {
        isStageClear = true;
        PanelSystem panelSys = inGameUI.transform.Find("IngamePanel").gameObject.GetComponent<PanelSystem>();
        panelSys.SetPanel(panelSys.stageClearPanel);

        SoundManager.Instance.StopBGMPlayer();
        SoundManager.Instance.PlaySound(SoundManager.SoundSpecific.OTHERUI, "Player_Game_Clear_Sound");
        ProcessStageClearInfo();
    }

    public void ProcessStageClearInfo()
    {
        bool temp = false;
        stageClearInfo.stageChpater[loadStageChapter-1].stageLevel[loadStageLevel-1].isClear = true;

        /* 업적 시스템 추가될 경우
        if(temp)
            stageClearInfo.stageChpater[loadStageChapter].stageLevel[loadStageLevel].achievement1 = true;
        if (temp)
            stageClearInfo.stageChpater[loadStageChapter].stageLevel[loadStageLevel].achievement2 = true;
        if (temp)
            stageClearInfo.stageChpater[loadStageChapter].stageLevel[loadStageLevel].achievement3 = true;
        */
    }
}