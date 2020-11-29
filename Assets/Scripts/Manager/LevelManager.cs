using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance { get; private set; }
    public class EnemyWave
    {
        public List<GameObject> oneWaveEnemies;
        public float enemySpawnDuration;

        public enum NextWaveTrigger
        {
            EnemyExterminated, // 해당 wave에서 마지막 적이 등장
            FirstEnemyDead, // 해당 wave의 첫번째 적이 사망
            HalfOfEnemyDead, // 해당 wave의 절반의 적이 사망
            LastEnemyDead, // 해당 wave의 마지막 적이 사망
            Timer // 특정 시간(timer)이 지난 이후
        }

        public NextWaveTrigger nextWaveTrigger;

        public float timer;
        public float breakTime;
        public int activeNum;
    }

    private Vector3 spawnerPosition;
    private GameObject waves;
    private List<EnemyWave> enemyWaveList;
    [HideInInspector] public bool isWaveSystemOn;
    [HideInInspector] public bool isWaveComplete;

    [Header("Preserved Variables")]
    public GameObject stageGameObject;
    public GameObject castle;
    public NavMeshSurface enemyNavMeshSurface;
    public NavMeshSurface checkerNavMeshSurface;

    [Header("Preserved StageData")]
    public StageScriptableObject stageData;

    [Header("Preserved Obstacle Prefabs")]
    public GameObject triangleObstaclePrefab;
    public GameObject squareObstaclePrefab;
    public GameObject pentagonObstaclePrefab;
    public GameObject hexagonObstaclePrefab;

    [Header("Preserved Tower Prefabs")]
    public GameObject triangleTowerPrefab;
    public GameObject squareTowerPrefab;
    public GameObject pentagonTowerPrefab;
    public GameObject hexagonTowerPrefab;

    [Header("Preserved Spawner & Destination Prefabs")]
    public GameObject spawnerPrefab;
    public GameObject destinationPrefab;

    [Header("Preserved Checker Prefabs")]
    public GameObject checkerPrefab;

    [Header("Preserved Enemy Prefabs")]
    public GameObject circleEnemyPrefab;
    public GameObject semiCircleEnemyPrefab;
    public GameObject sectorEnemyPrefab;
    public GameObject ellipseEnemyPrefab;
    public GameObject ringEnemyPrefab;

    [HideInInspector] public int m_enemyCount;

    private void Awake()
    {
        Init();
        LoadStage();
    }

    private void Start()
    {
        StartCoroutine(StartStage());
    }

    public void Init()
    {
        instance = this;

        PlayerControl.Instance.playerData.SetPlayer(this.stageData.startCost, this.stageData.playerLife);
        m_enemyCount = 0;
    }

    public int UpdateEnemyCount()
    {
        int enemyCount = 0;

        for(int i = 0; i< waves.transform.childCount; i++)
        {
            enemyCount += waves.transform.GetChild(i).childCount;
        }

        m_enemyCount = enemyCount;

        if (m_enemyCount <= 0 && PlayerControl.Instance.playerData.currentLife > 0)
        {
            GameManager.Instance.StageClear();
        }

        return m_enemyCount;
    }

    /* LoadStage
     * 1. Load obstacles.
     * 2. Load spawner and destination & Set active of overlabed wall to false.
     * 3. Bake enemy navmesh surface and checker navmesh surface.
     * 4. Load checker.
     * 5. Load wave.
     */
    public void LoadStage()
    {

        LoadObstacles();
        LoadTowers();
        LoadBlanks();
        LoadChecker();
        LoadWaves();
        //LoadTutorial();
        isWaveComplete = true;
    }

    /* LoadObstacles
     * 1. Load obstacles.
     * 2. Etc...
     */
    public void LoadObstacles()
    {
        GameObject obstacles = null;
        GameObject prefab = null;


        obstacles = new GameObject("Obstacles");
        obstacles.transform.SetParent(this.stageGameObject.transform);

        foreach (StageScriptableObject.ObstacleInfo info in this.stageData.baseObstacles)
        {
            if (info.obstacleSpecific
                == StageScriptableObject.ObstacleInfo.ObstacleSpecific.Triangle)
                prefab = this.triangleObstaclePrefab;
            else if (info.obstacleSpecific
                == StageScriptableObject.ObstacleInfo.ObstacleSpecific.Square)
                prefab = this.squareObstaclePrefab;
            else if (info.obstacleSpecific
                == StageScriptableObject.ObstacleInfo.ObstacleSpecific.Pentagon)
                prefab = this.pentagonObstaclePrefab;
            else // Hexagon
                prefab = this.hexagonObstaclePrefab;

            GameObject.Instantiate(prefab, info.position, info.rotation, obstacles.transform);
        }
    }

    /* LoadTowers
     * 1. Load towers.
     * 2. Etc...
     */
    public void LoadTowers()
    {
        GameObject towers = null;
        GameObject prefab = null;


        towers = new GameObject("Towers");
        towers.transform.SetParent(this.stageGameObject.transform);

        //foreach (StageScriptableObject.TowerInfo info in this.stageData.baseTowers)
        //{
        //    if (info.towerSpecific
        //        == StageScriptableObject.ObstacleInfo.TowerSpecific.Triangle)
        //        prefab = this.triangleObstaclePrefab;
        //    else if (info.towerSpecific
        //        == StageScriptableObject.ObstacleInfo.TowerSpecific.Square)
        //        prefab = this.squareObstaclePrefab;
        //    else if (info.towerSpecific
        //        == StageScriptableObject.ObstacleInfo.TowerSpecific.Pentagon)
        //        prefab = this.pentagonObstaclePrefab;
        //    else // Hexagon
        //        prefab = this.hexagonObstaclePrefab;

        //    GameObject.Instantiate(prefab, info.position, info.rotation, towers.transform);
        //}
    }

    /* LoadBlank
     * 1. Load spawner and destination.
     * 2. Set active of overlabed wall to false.
     * 3. Etc...
     */
    public void LoadBlanks()
    {
        StageScriptableObject.BlankInfo info = null;
        GameObject spawner = null;
        GameObject prefab = null;

        GameObject destination = null;


        //Load Spawner
        info = this.stageData.spawnerInfo;
        prefab = this.spawnerPrefab;
        spawner = GameObject.Instantiate(prefab, info.position, info.rotation, this.stageGameObject.transform);
        this.spawnerPosition = info.position;
        SelectBlankMesh(spawner, info);


        //Load Destination
        info = this.stageData.destinationInfo;
        prefab = this.destinationPrefab;
        destination = GameObject.Instantiate(prefab, info.position, info.rotation, this.stageGameObject.transform);
        SelectBlankMesh(destination, info);

        //Set active of overlabed wall to false
        SetWallActiveToFalse(spawner);
        SetWallActiveToFalse(destination);
    }

    /* SelectBlankMesh
     * 1. Set active of a appropriate mesh to true.
     * 2. Set active of a appropriate mesh to false.
     */
    public void SelectBlankMesh(GameObject blank, StageScriptableObject.BlankInfo blankInfo)
    {
        if (blankInfo.blankMeshType
            == StageScriptableObject.BlankInfo.BlankMeshType.Mesh1)
        {
            blank.transform.GetChild(0).gameObject.SetActive(true);
            blank.transform.GetChild(1).gameObject.SetActive(false);
            blank.transform.GetChild(2).gameObject.SetActive(false);
        }
        else if (blankInfo.blankMeshType
            == StageScriptableObject.BlankInfo.BlankMeshType.Mesh2)
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
    public void SetWallActiveToFalse(GameObject overlabedBlank)
    {
        bool breakFlag = false;

        Func<Vector3, Vector3, bool> isOverlabCheck = (blank, wall) =>
        {
            Func<float, float, bool> isOverlabCheckSpecificAxis = (blankX, wallX) =>
            {
                float error = 0.05f;
                if (wallX - error < blankX && blankX < wallX + error)
                    return true;

                return false;
            };

            if (isOverlabCheckSpecificAxis(blank.x, wall.x)
            && isOverlabCheckSpecificAxis(blank.y, wall.y)
            && isOverlabCheckSpecificAxis(blank.z, wall.z))
                return true;

            return false;
        };


        foreach (Transform wallLineTs in this.castle.GetComponentInChildren<Transform>())
        {
            foreach (Transform wallTs in wallLineTs.gameObject.GetComponentInChildren<Transform>())
            {
                if (isOverlabCheck(overlabedBlank.transform.position, wallTs.position))
                {
                    wallTs.gameObject.SetActive(false);
                    breakFlag = true;

                    break;
                }

                if (breakFlag)
                    break;

            }
        }
    }

    /* LoadChecker
     * 1. Load Checker.
     */
    public void LoadChecker()
    {
        GameObject checker;

        StageScriptableObject.BlankInfo info = null;

        info = this.stageData.spawnerInfo;

        Vector3 backward = info.rotation * Vector3.back;
        Vector3 positionVector = backward * 1.5f;

        checker = GameObject.Instantiate(this.checkerPrefab, info.position + positionVector, info.rotation, this.stageGameObject.transform).transform.Find("Checker").gameObject;

        BakeNavMeshSurfaces();

        checker.SetActive(true);
    }

    /* BakeNavMeshSurfaces
     * 1. Bake enemy navmesh surface.
     * 2. Bake checker navmesh surface.
     * 3. Etc..
    */
    public void BakeNavMeshSurfaces()
    {
        this.enemyNavMeshSurface.BuildNavMesh();
        this.checkerNavMeshSurface.BuildNavMesh();
    }

    /* StartStage
     * 1. Create waves.
     * 2. Create Enemies in one wave.
     * 3. repeat 2.
     * 4. etc...
     */
    public void LoadWaves()
    {
        GameObject wave = null;

        EnemyWave enemyWave = null;
        GameObject enemyPrefab = null;
        GameObject enemy = null;

        int waveValue = 1;
        int i = 0;


        this.waves = new GameObject("Waves");
        this.waves.transform.SetParent(this.gameObject.transform);

        this.enemyWaveList = new List<EnemyWave>();

        foreach (StageScriptableObject.EnemyWaveInfo enemyWaveInfo in this.stageData.baseEnemyWaves)
        {
            wave = new GameObject(string.Format("Wave{0}", waveValue));
            wave.transform.SetParent(this.waves.transform);

            enemyWave = new EnemyWave();
            enemyWave.oneWaveEnemies = new List<GameObject>();
            this.enemyWaveList.Add(enemyWave);

            enemyWave.enemySpawnDuration = enemyWaveInfo.enemySpawnDuration;
            enemyWave.nextWaveTrigger = (LevelManager.EnemyWave.NextWaveTrigger)enemyWaveInfo.nextWaveTrigger;
            enemyWave.timer = enemyWaveInfo.timer;
            enemyWave.breakTime = enemyWaveInfo.breakTime;
            enemyWave.activeNum = 0;

            foreach (StageScriptableObject.EnemyInfo enemyInfo in enemyWaveInfo.enemyInfoList)
            {
                for (i = 0; i < enemyInfo.count; i++)
                {
                    if (enemyInfo.enemySpecific
                    == StageScriptableObject.EnemyInfo.EnemySpecific.Circle)
                        enemyPrefab = this.circleEnemyPrefab;
                    else if (enemyInfo.enemySpecific
                        == StageScriptableObject.EnemyInfo.EnemySpecific.SemiCircle)
                        enemyPrefab = this.semiCircleEnemyPrefab;
                    else if (enemyInfo.enemySpecific
                        == StageScriptableObject.EnemyInfo.EnemySpecific.Sector)
                        enemyPrefab = this.sectorEnemyPrefab;
                    else if (enemyInfo.enemySpecific
                        == StageScriptableObject.EnemyInfo.EnemySpecific.Ellipse)
                        enemyPrefab = this.ellipseEnemyPrefab;
                    else // Ring
                        enemyPrefab = this.ringEnemyPrefab;

                    enemy = GameObject.Instantiate(enemyPrefab, spawnerPosition, Quaternion.identity, wave.transform);
                    enemy.GetComponent<EnemyData>().SetWaveNum(waveValue);
                    enemy.GetComponent<EnemyData>().ApplyWaveSystem();

                    enemyWave.oneWaveEnemies.Add(enemy);
                }
            }


            //if(enemyWaveInfo.isTutorialWave == false)
                waveValue++;
        }
    }

    public void LoadTutorial()
    {
        Transform tutorial = this.transform.Find("Tutorial");
        if (tutorial)
        {
            isWaveSystemOn = false;
            tutorial.gameObject.SetActive(true);
        }
        else
        {
            isWaveSystemOn = true;
        }
    }
    /* StartStage
     * 1. TakeBreakTime 1 & StartWave 1.
     * 2. TakeBreakTime 2 & StartWave 2.
     * 3. ...
     */
    private IEnumerator StartStage()
    {
        int waveValue = 1;

        foreach (EnemyWave enemyWave in this.enemyWaveList)
        {
            while (!(isWaveSystemOn && isWaveComplete))
            {
                yield return new WaitForSeconds(0.1f);
            }

            isWaveComplete = false;

            yield return StartCoroutine(TakeBreakTime(enemyWave.breakTime));

            StartCoroutine(StartWave(enemyWave));

            while (enemyWave.activeNum < enemyWave.oneWaveEnemies.Count)
            {
                yield return new WaitForSeconds(0.1f);
            }

            waveValue++;
            isWaveComplete = true;
            /*
            if(enemyWaveInfo.nextWaveTrigger
                == StageScriptableObject.EnemyWaveInfo.NextWaveTrigger.EnemyExterminated)
            {
                StartCoroutine(StartWave(enemyWaveInfo, waveValue));

                currentWaveEnemyNum = GetCurrentWaveEnemyNum(enemyWaveInfo);
                while (!(this.enemyWaveList[waveValue - 1].oneWaveEnemyList.Count == currentWaveEnemyNum))
                    break;   
            }
            else if (enemyWaveInfo.nextWaveTrigger
                == StageScriptableObject.EnemyWaveInfo.NextWaveTrigger.FirstEnemyDead)
            {
                StartCoroutine(StartWave(enemyWaveInfo, waveValue));

                while (!(this.enemyWaveList[waveValue - 1].oneWaveEnemyList[0] == null))
                    break;
            }
            else if (enemyWaveInfo.nextWaveTrigger
                == StageScriptableObject.EnemyWaveInfo.NextWaveTrigger.FirstEnemyDead)
            {

            }*/

        }
    }

    /* TakeBreakTime
     * 1. Take breaktime beafore the wave 
     */
    private IEnumerator TakeBreakTime(float breakTime)
    {
        while (true)
        {
            if (Mathf.Floor(breakTime) <= 0)
                break;
            else
            {
                breakTime -= Time.deltaTime;

                //Change Debug.Log to UI.text in UI system!
                //Debug.Log(string.Format("Timer: {0}", Mathf.Floor(breakTime)));
            }

            yield return null;
        }
    }

    /* StartWave
     * 1. Start a wave 
     */
    private IEnumerator StartWave(EnemyWave enemyWave)
    {
        foreach (GameObject enemy in enemyWave.oneWaveEnemies)
        {
            enemy.SetActive(true);
            enemyWave.activeNum++;
            yield return new WaitForSeconds(enemyWave.enemySpawnDuration);
        }
    }
}
