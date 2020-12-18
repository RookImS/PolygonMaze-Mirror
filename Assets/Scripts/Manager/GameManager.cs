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
            public bool isCurrent;
            public List<string> deckIDs;
        }

        public List<DeckInfo> deckInfoList;
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

        public List<StageLevelInfo> stageChapter;
    }

    [System.Serializable]
    public class SoundInfo
    {
        public bool isMuteBGM;
        public bool isMuteSE;
        public float bgmVolume;
        public float seVolume;
    }

    [HideInInspector] public Stack<string> sceneStack = new Stack<string>();  //BackKey 기능을 위해 씬 Buildindex를 저장하는 스택 

    private SkillsScriptableObject skillResource;
    [HideInInspector] public List<GameObject> skills;
    [HideInInspector] public List<List<GameObject>> deckList;
    [HideInInspector] public List<GameObject> currentDeck;

    private TutorialsScriptableObject tutorialResource;
    [HideInInspector] public List<TutorialObject> ingameTutorials;
    [HideInInspector] public List<TutorialObject> outgameTutorials;

    [HideInInspector] public AllDeckInfo allDeckInfo;
    [HideInInspector] public StageClearInfo stageClearInfo;
    [HideInInspector] public SoundInfo soundInfo;

    public bool isStart;
    private int loadStageChapter;
    private int loadStageLevel;

    [HideInInspector] public int chapterMax = 10;
    [HideInInspector] public int levelMax = 5;

    private new void Awake()
    {
        base.Awake();

        Init();
    }

    private void Start()
    {
        
    }

    public void Init()
    {
        Application.targetFrameRate = 60;
        SceneManager.sceneLoaded += OnSceneLoaded;

        loadStageChapter = 0;
        loadStageLevel = 0;

        LoadResource();
        LoadUserData();
    }

    private void LoadUserData()
    {
        LoadDeckInfos();
        LoadStageClearInfo();
        LoadSoundInfo();
    }

    private void LoadResource()
    {
        skillResource = (SkillsScriptableObject)Resources.Load("SkillList", typeof(SkillsScriptableObject));
        skills = skillResource.skillList;

        tutorialResource = (TutorialsScriptableObject)Resources.Load("TutorialList", typeof(TutorialsScriptableObject));
        ingameTutorials = tutorialResource.ingameTutorialList;
        outgameTutorials = tutorialResource.outgameTutorialList;
    }

    private void MakeEmptyDeck()
    {
        deckList = new List<List<GameObject>>();
        allDeckInfo = new AllDeckInfo();
        allDeckInfo.deckInfoList = new List<AllDeckInfo.DeckInfo>();
        for (int i = 0; i < 3; i++)
        {
            deckList.Add(new List<GameObject>());
            allDeckInfo.deckInfoList.Add(new AllDeckInfo.DeckInfo());
            allDeckInfo.deckInfoList[i].deckIDs = new List<string>();
            allDeckInfo.deckInfoList[i].isCurrent = false;
            for (int j = 0; j < 3; j++)
            {
                deckList[i].Add(null);
                allDeckInfo.deckInfoList[i].deckIDs.Add("space");
            }
        }
    }

    private void LoadDeckInfos()
    {
        string dir;
        string path;

        MakeEmptyDeck();

        if (Application.platform == RuntimePlatform.Android)
            dir = Application.persistentDataPath + string.Format("/UserData/DeckData");
        else
            dir = string.Format("Assets/UserData/DeckData");

        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);

        for (int i = 0; i < 3; i++)
        {
            path = dir + string.Format("/Deck{0}.json", i);

            try
            {
                if (File.Exists(path))
                {
                    string jsonData = File.ReadAllText(path);
                    this.allDeckInfo.deckInfoList[i] = JsonUtility.FromJson<AllDeckInfo.DeckInfo>(jsonData);
                }
                else
                {
                    if (i == 0)
                    {
                        List<GameObject> defaultDeck = new List<GameObject>();
                        defaultDeck.Add(skills[0]);
                        defaultDeck.Add(skills[1]);
                        defaultDeck.Add(skills[2]);
                        deckList[0] = defaultDeck;
                        currentDeck = deckList[0];
                    }

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

            if (allDeckInfo.deckInfoList[i].isCurrent)
                currentDeck = deckList[i];

            for (int j = 0; j < 3; j++)
            {
                for (int k = 0; k < skills.Count; k++)
                {
                    if (skills[k].GetComponent<Skill>().id
                        == this.allDeckInfo.deckInfoList[i].deckIDs[j])
                        deckList[i][j] = skills[k];
                    else if ("space"
                        == this.allDeckInfo.deckInfoList[i].deckIDs[j])
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
                allDeckInfo.deckInfoList[order].deckIDs[i] = "space";   
            }
        }
        else
        {
            for (int i = 0; i < 3; i++)
            {
                allDeckInfo.deckInfoList[order].deckIDs[i] = deckList[order][i].GetComponent<Skill>().id;
            }
        }

        if (deckList[order] == currentDeck)
            allDeckInfo.deckInfoList[order].isCurrent = true;
        else
            allDeckInfo.deckInfoList[order].isCurrent = false;

        string jsonData = JsonUtility.ToJson(allDeckInfo.deckInfoList[order]);
        string dir;
        string path;

        if (Application.platform == RuntimePlatform.Android)
            dir = Application.persistentDataPath + string.Format("/UserData/DeckData");
        else
            dir = string.Format("Assets/UserData/DeckData");

        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);

        path = dir + string.Format("/Deck{0}.json", order);

        try
        {
            File.WriteAllText(path, jsonData);
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
    public void MakeEmptyStageClearInfo()
    {
        stageClearInfo = new StageClearInfo();
        stageClearInfo.stageChapter = new List<StageClearInfo.StageLevelInfo>();

        for (int i = 0; i < chapterMax; i++)
        {
            stageClearInfo.stageChapter.Add(new StageClearInfo.StageLevelInfo());
            stageClearInfo.stageChapter[i].stageLevel = new List<StageClearInfo.StageLevelInfo.StageInfo>();

            for (int j = 0; j < levelMax; j++)
            {
                stageClearInfo.stageChapter[i].stageLevel.Add(new StageClearInfo.StageLevelInfo.StageInfo());
                stageClearInfo.stageChapter[i].stageLevel[j].isClear = false;
                stageClearInfo.stageChapter[i].stageLevel[j].achievement1 = false;
                stageClearInfo.stageChapter[i].stageLevel[j].achievement2 = false;
                stageClearInfo.stageChapter[i].stageLevel[j].achievement3 = false;
            }
        }
    }

    public void LoadStageClearInfo()
    {
        string dir;
        string path;

        MakeEmptyStageClearInfo();

        if (Application.platform == RuntimePlatform.Android)
            dir = Application.persistentDataPath + string.Format("/UserData/StageClearData");
        else
            dir = string.Format("Assets/UserData/StageClearData");

        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);

        path = dir + string.Format("/StageClearData.json");

        try
        {
            if (File.Exists(path))
            {
                string jsonData = File.ReadAllText(path);
                this.stageClearInfo = JsonUtility.FromJson<StageClearInfo>(jsonData);
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
    

    public void SaveStageClearInfo()
    {
        string jsonData = JsonUtility.ToJson(stageClearInfo);
        string dir;
        string path;
        
        if (Application.platform == RuntimePlatform.Android)
            dir = Application.persistentDataPath + string.Format("/UserData/StageClearData");
        else
            dir = string.Format("Assets/UserData/StageClearData");

        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);

        path = dir + string.Format("/StageClearData.json");

        try
        {
            File.WriteAllText(path, jsonData);
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

    public void LoadSoundInfo()
    {

    }

    public void SaveSoundInfo()
    {

    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);

        if (SoundManager.instance != null)
        {
            if (sceneName == "StageScene")
                SoundManager.instance.PlayBGM("InGame_BGM");
            else
                SoundManager.instance.PlayBGM("OutGame_BGM");
        }

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

    public new void OnApplicationQuit()
    {
        base.OnApplicationQuit();
        SaveStageClearInfo();
        for (int i = 0; i < 3; i++)
            SaveDeckInfos(i);
    }


    private void OnApplicationPause(bool pause)
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

        if (SoundManager.instance != null)
        {
            SoundManager.Instance.StopBGMPlayer();
            SoundManager.Instance.PlaySound(SoundManager.SoundSpecific.OTHERUI, "Player_Game_Over_Sound");
        }
    }

    public void StageClear()
    {
        isStageClear = true;
        PanelSystem panelSys = inGameUI.transform.Find("IngamePanel").gameObject.GetComponent<PanelSystem>();
        panelSys.SetPanel(panelSys.stageClearPanel);

        if (SoundManager.instance != null)
        {
            SoundManager.Instance.StopBGMPlayer();
            SoundManager.Instance.PlaySound(SoundManager.SoundSpecific.OTHERUI, "Player_Game_Clear_Sound");
        }
        ProcessStageClearInfo();
        SaveStageClearInfo();
    }

    public void ProcessStageClearInfo()
    {
        bool temp = false;
        stageClearInfo.stageChapter[loadStageChapter-1].stageLevel[loadStageLevel-1].isClear = true;

        /* 업적 시스템 추가될 경우
        if(temp)
            stageClearInfo.stageChapter[loadStageChapter].stageLevel[loadStageLevel].achievement1 = true;
        if (temp)
            stageClearInfo.stageChapter[loadStageChapter].stageLevel[loadStageLevel].achievement2 = true;
        if (temp)
            stageClearInfo.stageChapter[loadStageChapter].stageLevel[loadStageLevel].achievement3 = true;
        */
    }
}
