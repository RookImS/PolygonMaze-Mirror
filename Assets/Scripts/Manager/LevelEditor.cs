using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Security.Cryptography;

public class LevelEditor : MonoBehaviour
{
    public class BlankTempInfo
    {
        public GameObject blank;
        public GameObject disabledWall;
    }

    public static LevelEditor instance { get; private set; }
    private StageData stageData;
    private int playerLife;
    private int startCost;
    private List<GameObject> obstacleList;
    private List<BlankTempInfo> spawnerList;
    private List<BlankTempInfo> destinationList;
    private List<StageData.EnemyWaveInfo> enemyWaveInfoList;
    private int enemyNum;

    public void Awake()
    {
        Init();
    }

    public void Init()
    {
        instance = this;
        stageData = new StageData();
        playerLife = -1;
        startCost = -1;
        obstacleList = new List<GameObject>();
        spawnerList = new List<BlankTempInfo>();
        destinationList = new List<BlankTempInfo>();
        enemyWaveInfoList = new List<StageData.EnemyWaveInfo>();
        enemyNum = 0;
    }



    public int GetPlayerLife()
    {
        return this.playerLife;
    }

    public int GetStartCost()
    {
        return this.startCost;
    }

    public void SetPlayerLife(int playerLife)
    {
        this.playerLife = playerLife;
    }

    public void SetStartCost(int startCost)
    {
        this.startCost = startCost;
    }

    public List<BlankTempInfo> GetSpawnerList()
    {
        return spawnerList;
    }

    public List<BlankTempInfo> GetDestinationList()
    {
        return destinationList;
    }

    public void AddObstacle(GameObject obstacle)
    {
        this.obstacleList.Add(obstacle);
    }

    public void DeleteObstacle(GameObject obstacle)
    {
        this.obstacleList.Remove(obstacle);
    }

    /*
     * obstacle info를 stageData에 update
     */
    public void UpdateObstacleInfo()
    {
        this.stageData.UpdateObstacleInfo(this.obstacleList);
    }

    public void AddSpawner(GameObject spawner, GameObject disabledwall)
    {
        if (this.spawnerList.Find(x => x.blank == spawner) == null
            && this.spawnerList.Find(x => x.disabledWall == disabledwall) == null)
        {
            BlankTempInfo blankTempInfo = new BlankTempInfo();
            blankTempInfo.blank = spawner;
            blankTempInfo.disabledWall = disabledwall;
            this.spawnerList.Add(blankTempInfo);
        }
        else
        {
            Debug.Log("Error in AddSpawner of LevelEditor.cs");
        }
    }

    public GameObject DeleteSpawner(GameObject spawner)
    {
        GameObject disabledWall = null;
        BlankTempInfo blankTempInfo = this.spawnerList.Find(x => x.blank == spawner);
        if (blankTempInfo != null)
        {
            this.spawnerList.Remove(blankTempInfo);
            disabledWall = blankTempInfo.disabledWall;
        }
        else
        {
            Debug.Log("Error in DeleteSpawner of LevelEditor.cs");
        }

        return disabledWall;
    }
    /*
     * blank(spawner,destination) info를 stageData에 update
     */
    public void UpdateSpawnerInfo()
    {
        stageData.UpdateBlankInfo(this.spawnerList);
    }

    public void AddDestination(GameObject destination, GameObject disabledwall)
    {
        if (this.destinationList.Find(x => x.blank == destination) == null
            && this.destinationList.Find(x => x.disabledWall == disabledwall) == null)
        {
            BlankTempInfo blankTempInfo = new BlankTempInfo();
            blankTempInfo.blank = destination;
            blankTempInfo.disabledWall = disabledwall;
            this.destinationList.Add(blankTempInfo);
        }
        else
        {
            Debug.Log("Error in AddDestination of LevelEditor.cs");
        }
    }

    public GameObject DeleteDestination(GameObject destination)
    {
        GameObject disabledWall = null;
        BlankTempInfo blankTempInfo = this.destinationList.Find(x => x.blank == destination);
        if (blankTempInfo != null)
        {
            this.destinationList.Remove(blankTempInfo);
            disabledWall = blankTempInfo.disabledWall;
        }
        else
        {
            Debug.Log("Error in DeleteDestination of LevelEditor.cs");
        }

        return disabledWall;
    }
    /*
     * blank(spawner,destination) info를 stageData에 update
     */
    public void UpdateDestinationInfo()
    {
        stageData.UpdateBlankInfo(this.destinationList);
    }


    public void AddEnemyWaveInfoList(StageData.EnemyWaveInfo enemyWaveInfo)
    {
        this.enemyWaveInfoList.Add(enemyWaveInfo);
        foreach(StageData.EnemyInfo enemyInfo in enemyWaveInfo.enemyInfos)
        {
            this.enemyNum += enemyInfo.count;
        }
    }

    public void DeleteEnemyWaveInfoList(StageData.EnemyWaveInfo enemyWaveInfo)
    {
        this.enemyWaveInfoList.Remove(enemyWaveInfo);
        foreach (StageData.EnemyInfo enemyInfo in enemyWaveInfo.enemyInfos)
        {
            this.enemyNum -= enemyInfo.count;
        }
    }

    /*
     * enemyWave info를 stageData에 update
     */
    public void UpdateEnemyWaveInfo()
    {
        stageData.UpdateEnemyWaveInfo(this.enemyWaveInfoList);
    }

    public void UpdateEenmyNum()
    {
        stageData.UpdateEnemyNum(this.enemyNum);
    }

    /*
     * Editor에 적용한 내용을 stageData에 update
     */
    public void UpdateStageData()
    {
        UpdateObstacleInfo();
        UpdateSpawnerInfo();
        UpdateDestinationInfo();
        UpdateEnemyWaveInfo();
        UpdateEenmyNum();
    }

    /*
     * stage data를 json형식으로 저장
     */
    public void SaveStageData()
    {
        UpdateStageData();

        string jsonData = JsonUtility.ToJson(stageData);
        string path = string.Format("Assets/stageData/{0}-{1}.json", stageData.stageName, stageData.stageLevel);

        System.IO.FileInfo file = new System.IO.FileInfo(path);
        file.Directory.Create();

        try
        {
            if (File.Exists(path))
            {
                // 동일이름의 파일 존재
                // stage name 재입력 event
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
}