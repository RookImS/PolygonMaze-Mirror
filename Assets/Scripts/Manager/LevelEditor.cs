using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Security.Cryptography;
using UnityEngine.AI;
using TreeEditor;

public class LevelEditor : MonoBehaviour
{
    public class BlankTempInfo
    {
        public StageData.BlankInfo.BlankMeshType blankMeshType;
        public GameObject blank;
        public GameObject checkerWithGround;
        public GameObject checker;
        public GameObject disabledWall;
    }

    public static LevelEditor instance { get; private set; }

    
    private StageData stageData;
    private int stageChapter;
    private int stageLevel;
    private int playerLife;
    private int startCost;
    private BlankTempInfo spawnerInfo;
    private BlankTempInfo destinationInfo;
    private GameObject obstacles;
    private List<GameObject> obstacleList;
    private List<StageData.EnemyWaveInfo> enemyWaveInfoList;
    private int enemyNum;
    private StageData.ReadyFlag readyFlag;

    private int navMeshUpdateFlag;


    [Header("Preserved Variables")]
    public GameObject stageGameObject;
    public NavMeshSurface enemyNavMeshSurface;
    public NavMeshSurface checkerNavMeshSurface;

    [Header("Preserved Checker Prefabs")]
    public GameObject checkerWithGroundPrefab;


    public void Awake()
    {
        instance = this;
        Init();
    }


    public void Start()
    {
        if (GameManager.Instance.GetLoadStageChapter() != 0
            && GameManager.Instance.GetLoadStageLevel() != 0)
        {
            if (LoadStageData())
                LoadLevelEditor();

            GameManager.Instance.SetLoadStageChapter(0);
            GameManager.Instance.SetLoadStageLevel(0);
        }

        this.stageData = new StageData();
    }

    public void Update()
    {
        if (navMeshUpdateFlag != 0)
            UpdateNavMeshSurfaces();
    }

    public void Init()
    {
        stageChapter = 0;
        stageLevel = 0;
        playerLife = 0;
        startCost = 0;
        spawnerInfo = null;
        destinationInfo = null;

        if (obstacles != null)
            Destroy(obstacles);
        obstacles = new GameObject("Obstacles");
        obstacles.transform.SetParent(this.stageGameObject.transform);
        obstacleList = new List<GameObject>();

        enemyWaveInfoList = new List<StageData.EnemyWaveInfo>();
        enemyNum = 0;
        readyFlag = new StageData.ReadyFlag();

        navMeshUpdateFlag = 0;
    }


    public int GetPlayerLife()
    {
        return this.playerLife;
    }
    public void SetPlayerLife(int playerLife)
    {
        this.playerLife = playerLife;
    }

    public int GetStartCost()
    {
        return this.startCost;
    }
    public void SetStartCost(int startCost)
    {
        this.startCost = startCost;
    }

    public BlankTempInfo GetSpawnerInfo()
    {
        return this.spawnerInfo;
    }

    public void SetSpawnerInfo(GameObject spawner, GameObject checkerWithGround, GameObject checker, GameObject disabledwall)
    {
        if (this.spawnerInfo != null)
            DeleteSpawner();

        this.spawnerInfo = new BlankTempInfo();

        if (spawner.transform.GetChild(0).gameObject.activeSelf == true)
            this.spawnerInfo.blankMeshType = StageData.BlankInfo.BlankMeshType.Mesh1;
        else if (spawner.transform.GetChild(1).gameObject.activeSelf == true)
            this.spawnerInfo.blankMeshType = StageData.BlankInfo.BlankMeshType.Mesh2;
        else
            this.spawnerInfo.blankMeshType = StageData.BlankInfo.BlankMeshType.Mesh3;

        this.spawnerInfo.blank = spawner;
        this.spawnerInfo.checkerWithGround = checkerWithGround;
        this.spawnerInfo.checker = checker;
        this.spawnerInfo.disabledWall = disabledwall;
    }

    public void DeleteSpawner()
    {
        if (this.spawnerInfo != null)
        {
            Destroy(this.spawnerInfo.blank);
            Destroy(this.spawnerInfo.checkerWithGround);
            this.spawnerInfo.disabledWall.SetActive(true);
            this.spawnerInfo = null;
        }
        else
            Debug.Log("Error in DeleteSpawner of LevelEditor.cs");
    }

    public BlankTempInfo GetDestinationInfo()
    {
        return this.destinationInfo;
    }

    public void SetDestinationInfo(GameObject destination, GameObject disabledwall)
    {
        if (this.destinationInfo != null)
            DeleteDestination();

        this.destinationInfo = new BlankTempInfo();

        if (destination.transform.GetChild(0).gameObject.activeSelf == true)
            this.destinationInfo.blankMeshType = StageData.BlankInfo.BlankMeshType.Mesh1;
        else if (destination.transform.GetChild(1).gameObject.activeSelf == true)
            this.destinationInfo.blankMeshType = StageData.BlankInfo.BlankMeshType.Mesh2;
        else
            this.destinationInfo.blankMeshType = StageData.BlankInfo.BlankMeshType.Mesh3;

        this.destinationInfo.blank = destination;
        this.destinationInfo.checkerWithGround = null;
        this.destinationInfo.checker = null;
        this.destinationInfo.disabledWall = disabledwall;
    }

    public List<GameObject> GetObstacleList()
    {
        return this.obstacleList;
    }

    public List<StageData.EnemyWaveInfo> GetEnemyWaveInfoList()
    {
        return this.enemyWaveInfoList;
    }

    public void DeleteDestination()
    {
        if (destinationInfo != null)
        {
            Destroy(this.destinationInfo.blank);
            this.destinationInfo.disabledWall.SetActive(true);
            this.destinationInfo = null;
        }
        else
            Debug.Log("Error in DeleteDestination of LevelEditor.cs");
    }

    public void AddObstacle(GameObject obstacle)
    {
        this.obstacleList.Add(obstacle);
    }

    public void DeleteObstacle(GameObject obstacle)
    {
        List<ObstacleData.Neighbor> list = null;

        foreach (ObstacleData.Neighbor neighbor in obstacle.GetComponent<ObstacleData>().NeighborList)
        {
            list = neighbor.obj.GetComponent<ObstacleData>().NeighborList;
            list.Remove(list.Find((x) => (x.obj == obstacle)));

            neighbor.collider.enabled = true;
        }

        this.obstacleList.Remove(obstacle);
        Destroy(obstacle);
    }

    public void AddEnemyWaveInfo(StageData.EnemyWaveInfo enemyWaveInfo)
    {
        this.enemyWaveInfoList.Add(enemyWaveInfo);
    }

    public void DeleteEnemyWaveInfo(StageData.EnemyWaveInfo enemyWaveInfo)
    {
        this.enemyWaveInfoList.Remove(enemyWaveInfo);
    }

    public int GetStageChapter()
    {
        return this.stageChapter;
    }

    public void SetStageChapter(int stageChapter)
    {
        this.stageChapter = stageChapter;
    }

    public int GetStageLevel()
    {
        return this.stageLevel;
    }

    public void SetStageLevel(int stageLevel)
    {
        this.stageLevel = stageLevel;
    }

    public void UpdatePlayerInfo()
    {
        this.stageData.UpdatePlayerInfo(this.playerLife, this.startCost);
    }

    public void UpdateSpawnerInfo()
    {
        this.stageData.UpdateBlankInfo(spawnerInfo);
    }

    public void UpdateDestinationInfo()
    {
        this.stageData.UpdateBlankInfo(destinationInfo);
    }

    /*
     * obstacle info를 stageData에 update
     */
    public void UpdateObstacleInfo()
    {
        this.stageData.UpdateObstacleInfo(this.obstacleList);
        this.stageData.SortObstacleInfo();
    }

    /*
     * enemyWave info를 stageData에 update
     */
    public void UpdateEnemyWaveInfo()
    {
        this.stageData.UpdateEnemyWaveInfo(this.enemyWaveInfoList);
    }

    public void UpdateEenmyNum()
    {
        this.enemyNum = 0;

        foreach (StageData.EnemyWaveInfo enemyWaveInfo in this.enemyWaveInfoList)
            this.enemyNum += enemyWaveInfo.enemyOneWave.Count;
    
        this.stageData.UpdateEnemyNum(this.enemyNum);
    }

    public void UpdateStageChapterAndLevel()
    {
        this.stageData.UpdateStageChapterAndLevel(this.stageChapter, this.stageLevel);
    }

    public void UpdateReadyFlag()
    {
        this.stageData.UpdateReadyFlag(this.readyFlag);
    }

    /*
     * Editor에 적용한 내용을 stageData에 update
     */
    public void UpdateStageData()
    {
        UpdatePlayerInfo();
        UpdateSpawnerInfo();
        UpdateDestinationInfo();
        UpdateObstacleInfo();
        UpdateEnemyWaveInfo();
        UpdateEenmyNum();
        UpdateStageChapterAndLevel();
        UpdateReadyFlag();
    }

    /*
     * stage data를 json형식으로 저장
     */
    public void SaveStageData()
    {
        UpdateStageData();

        string jsonData = JsonUtility.ToJson(stageData);
        string path = string.Format("Assets/StageData/{0}-{1}.json", stageData.stageChapter, stageData.stageLevel);

        System.IO.FileInfo file = new System.IO.FileInfo(path);
        file.Directory.Create();

        try
        {
            if (File.Exists(path))
            {
                // 동일이름의 파일 존재
                // 덮어씌우실? 마실? 재입력 event
            }
            else
            {
                File.WriteAllText(file.FullName, jsonData);
            }
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

    public void CreateChecker(GameObject currentBlankObject)
    {
        Transform info = currentBlankObject.transform;

        Vector3 backward = info.rotation * Vector3.back;
        Vector3 positionVector = backward * 1.5f;

        GameObject checkerWithGround
            = GameObject
            .Instantiate(this.checkerWithGroundPrefab
            , info.position + positionVector
            , info.rotation,
            this.stageGameObject.transform);

        GameObject checker = checkerWithGround.transform.Find("Checker").gameObject;

        LevelEditorUISystem
            .instance
            .GetSpawnerCreateSettingUISystem()
            .SetCheckerWithGroundObject(checkerWithGround);

        LevelEditorUISystem
            .instance
            .GetSpawnerCreateSettingUISystem()
            .SetCheckerObject(checker);

        BakeNavMeshSurfaces();

        checker.SetActive(true);
    }

    public void BakeNavMeshSurfaces()
    {
        this.enemyNavMeshSurface.BuildNavMesh();
        this.checkerNavMeshSurface.BuildNavMesh();
    }

    public void SetNavMeshUpdateFlag()
    {
        this.navMeshUpdateFlag = 2;
    }

    public void UpdateNavMeshSurfaces()
    {
        if (navMeshUpdateFlag == 1)
        {
            enemyNavMeshSurface.UpdateNavMesh(enemyNavMeshSurface.navMeshData);
            checkerNavMeshSurface.UpdateNavMesh(checkerNavMeshSurface.navMeshData);
            spawnerInfo.checker.GetComponent<CheckerBehaviour>().ApplyPath();
        }

        navMeshUpdateFlag--;
    }

    public StageData.ReadyFlag GetReadyFlag()
    {
        return this.readyFlag;
    }

    public bool LoadStageData()
    {
        int loadStageChapter = GameManager.Instance.GetLoadStageChapter();
        int loadStageLevel = GameManager.Instance.GetLoadStageLevel();

        string path = string.Format("Assets/StageData/{0}-{1}.json", loadStageChapter, loadStageLevel);
        System.IO.FileInfo file = new System.IO.FileInfo(path);

        try
        {
            if (File.Exists(path))
            {
                string jsonData = File.ReadAllText(path);
                this.stageData = JsonUtility.FromJson<StageData>(jsonData);

                return true;
            }
            else
            {
                // 동일이름의 파일 존재하지 않음
                // 재입력 event

                return false;
            }
        }
        catch (System.ArgumentException e1)
        {
            Debug.Log(e1.Message);
            // stage name 재입력 event

            return false;
        }
        catch (System.Exception e2)
        {
            Debug.Log(e2.Message);
            // IOException or UnauthorizedAccessException

            return false;
        } 
    }

    public void LoadLevelEditor()
    {
        LoadStageChapterAndLevel();
        LoadPlayerLifeAndStartCost();
        LoadBlanks();
        LoadObstacles();
        LoadEnemyWaves();
    }

    public void LoadStageChapterAndLevel()
    {
        if(this.stageData.stageChapter != 0)
            this.stageChapter = this.stageData.stageChapter;
        if (this.stageData.stageLevel != 0)
            this.stageLevel = this.stageData.stageLevel;

        SaveLoadExitUISystem system = LevelEditorUISystem.instance.GetSaveExitUISystem();
        system.Load();
    }

    public void LoadPlayerLifeAndStartCost()
    {
        if (this.stageData.playerLife != 0)
            this.playerLife = this.stageData.playerLife;
        if (this.stageData.startCost != 0)
            this.startCost = this.stageData.startCost;

        PlayerSettingUISystem system = LevelEditorUISystem.instance.GetPlayerSettingUISystem();
        system.Load();
    }

    public void LoadBlanks()
    {
        BlankCreateButtonUISystem spawnerSystem = LevelEditorUISystem.instance.GetSpawnerCreateSettingUISystem();
        BlankCreateButtonUISystem destinationSystem = LevelEditorUISystem.instance.GetDestinationCreateSettingUISystem();

        StageData.BlankInfo info = null;
        GameObject spawner = null;
        GameObject prefab = null;

        GameObject destination = null;


        //Load Spawner
        info = this.stageData.spawnerInfo;
        prefab = LevelEditorUISystem.instance.GetSpawnerCreateSettingUISystem().blankPrefab;
        spawner = GameObject.Instantiate(prefab, info.position, info.rotation, this.stageGameObject.transform);
        SelectBlankMesh(spawner, info);

        this.spawnerInfo = new BlankTempInfo();
        this.spawnerInfo.blank = spawner;
        this.spawnerInfo.blankMeshType = info.blankMeshType;

        //Load Destination
        info = this.stageData.destinationInfo;
        prefab = LevelEditorUISystem.instance.GetDestinationCreateSettingUISystem().blankPrefab;
        destination = GameObject.Instantiate(prefab, info.position, info.rotation, this.stageGameObject.transform);
        SelectBlankMesh(destination, info);

        this.destinationInfo = new BlankTempInfo();
        this.destinationInfo.blank = destination;
        this.destinationInfo.blankMeshType = info.blankMeshType;

        //Set active of overlabed wall to false
        this.spawnerInfo.disabledWall = SetWallActiveToFalse(spawner);
        this.destinationInfo.disabledWall = SetWallActiveToFalse(destination);

        LoadChecker();

        spawnerSystem.Load(spawner);
        destinationSystem.Load(destination);
    }

    /* SelectBlankMesh
     * 1. Set active of a appropriate mesh to true.
     * 2. Set active of a appropriate mesh to false.
     */
    public void SelectBlankMesh(GameObject blank, StageData.BlankInfo blankInfo)
    {
        if (blankInfo.blankMeshType
            == StageData.BlankInfo.BlankMeshType.Mesh1)
        {
            blank.transform.GetChild(0).gameObject.SetActive(true);
            blank.transform.GetChild(1).gameObject.SetActive(false);
            blank.transform.GetChild(2).gameObject.SetActive(false);
        }
        else if (blankInfo.blankMeshType
            == StageData.BlankInfo.BlankMeshType.Mesh2)
        {
            blank.transform.GetChild(0).gameObject.SetActive(false);
            blank.transform.GetChild(1).gameObject.SetActive(true);
            blank.transform.GetChild(2).gameObject.SetActive(false);
        }
        else // Mesh3
        {
            blank.transform.GetChild(0).gameObject.SetActive(false);
            blank.transform.GetChild(1).gameObject.SetActive(false);
            blank.transform.GetChild(2).gameObject.SetActive(true);
        }
    }

    /* SetWallActiveToFalse
     * 1. Check if the wall overlabs with blank(spawner or destination).
     * 2. Set active of wall to false if overlabed.
     */
    public GameObject SetWallActiveToFalse(GameObject overlabedBlank)
    {
        Func<Vector3, Vector3, bool> isOverlapCheck = (blank, wall) =>
        {
            Func<float, float, bool> isOverlapCheckSpecificAxis = (blankX, wallX) =>
            {
                float error = 0.05f;
                if (wallX - error < blankX && blankX < wallX + error)
                    return true;

                return false;
            };

            if (isOverlapCheckSpecificAxis(blank.x, wall.x)
            && isOverlapCheckSpecificAxis(blank.y, wall.y)
            && isOverlapCheckSpecificAxis(blank.z, wall.z))
                return true;

            return false;
        };


        foreach (Transform wallLineTs in this.stageGameObject.transform.GetChild(0))
        {
            foreach (Transform wallTs in wallLineTs.gameObject.GetComponentInChildren<Transform>())
            {
                if (isOverlapCheck(overlabedBlank.transform.position, wallTs.position))
                {
                    wallTs.gameObject.SetActive(false);
                    return wallTs.gameObject;
                }
            }
        }

        return null;
    }

    /* LoadChecker
     * 1. Load Checker.
     */
    public void LoadChecker()
    {
        GameObject checkerWithGround;
        GameObject checker;

        StageData.BlankInfo info = null;

        info = this.stageData.spawnerInfo;

        Vector3 backward = info.rotation * Vector3.back;
        Vector3 positionVector = backward * 1.5f;

        checkerWithGround
            = GameObject
            .Instantiate(this.checkerWithGroundPrefab, info.position + positionVector, info.rotation, this.stageGameObject.transform);

        checker = checkerWithGround.transform.GetChild(0).gameObject;
        
        this.spawnerInfo.checkerWithGround = checkerWithGround;
        this.spawnerInfo.checker = checker;

        BakeNavMeshSurfaces();
        checker.SetActive(true);
    }

    public void LoadObstacles()
    {
        ObstacleCreateButtonUISystem createSystem = null;
        ObstacleSettingUISystem settingSystem = LevelEditorUISystem.instance.GetObstacleSettingUISystem();
        GameObject prefab = null;

        foreach (StageData.ObstacleInfo info in this.stageData.obstacles)
        {
            createSystem = LevelEditorUISystem.instance.GetObstacleCreateButtonUISystem(info.obstacleSpecific);
            prefab = createSystem.realObstacle;

            this.obstacleList.Add(GameObject.Instantiate(prefab, info.position, info.rotation, obstacles.transform));
        }

        settingSystem.Load();
        LevelEditor.instance.SetNavMeshUpdateFlag();
    }

    public void LoadEnemyWaves()
    {
        EnemyWaveSettingUISystem system = LevelEditorUISystem.instance.GetEnemyWaveSettingUISystem();
        StageData.EnemyWaveInfo enemyWaveInfo = null;
        StageData.Enemies enemies = null;

        for (int i = 0; i < this.stageData.enemyWaveInfos.Count; i++)
        {
            enemyWaveInfo = new StageData.EnemyWaveInfo();
            enemyWaveInfo.enemyOneWave = new List<StageData.Enemies>();

            for (int j = 0; j < this.stageData.enemyWaveInfos[i].enemyOneWave.Count; j++)
            {
                enemies = new StageData.Enemies();
                enemies.enemySpecific = this.stageData.enemyWaveInfos[i].enemyOneWave[j].enemySpecific;
                enemies.count = this.stageData.enemyWaveInfos[i].enemyOneWave[j].count;

                enemyWaveInfo.enemyOneWave.Add(enemies);
            }

            enemyWaveInfo.enemySpawnDuration = this.stageData.enemyWaveInfos[i].enemySpawnDuration;
            enemyWaveInfo.nextWaveTrigger = this.stageData.enemyWaveInfos[i].nextWaveTrigger;
            enemyWaveInfo.breakTime = this.stageData.enemyWaveInfos[i].breakTime;

            enemyWaveInfoList.Add(enemyWaveInfo);
        }

        system.Load();
    }
}