using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

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
    private List<BlankTempInfo> blankList;
    private List<StageData.EnemyWaveInfo> enemyWaveInfoList;

    public void Awake()
    {
        Init();
    }

    public void Init()
    {
        instance = this;
        stageData = new StageData();
        playerLife = 10;
        startCost = 200;
        obstacleList = new List<GameObject>();
        blankList = new List<BlankTempInfo>();
        enemyWaveInfoList = new List<StageData.EnemyWaveInfo>();
    }

    public void SetPlayerLife(int playerLife)
    {
        this.playerLife = playerLife;
    }

    public void SetStartCost(int startCost)
    {
        this.startCost = startCost;
    }

    public void AddObstacle(GameObject obstacle)
    {
        obstacleList.Add(obstacle);
    }

    public void DeleteObstacle(GameObject obstacle)
    {
        obstacleList.Remove(obstacle);
    }

    /*
     * obstacle info를 stageData에 update
     */
    public void UpdateObstacleInfo()
    {
        stageData.UpdateObstacleInfo(obstacleList);
    }

    public void AddBlank(GameObject blank, GameObject disabledwall)
    {
        if (blankList.Find(x => x.blank == blank) == null && blankList.Find(x => x.disabledWall == disabledwall) == null)
        {
            BlankTempInfo blankTempInfo = new BlankTempInfo();
            blankTempInfo.blank = blank;
            blankTempInfo.disabledWall = disabledwall;
            blankList.Add(blankTempInfo);
        }
        else
        {
            Debug.Log("Error in AddBlank of LevelEditor.cs");
        }
    }

    public GameObject DeleteBlank(GameObject blank)
    {
        GameObject disabledWall = null;
        BlankTempInfo blankTempInfo = blankList.Find(x => x.blank == blank);
        if (blankTempInfo != null)
        {
            blankList.Remove(blankTempInfo);
            disabledWall = blankTempInfo.disabledWall;
        }
        else
        {
            Debug.Log("Error in DeleteBlank of LevelEditor.cs");
        }

        return disabledWall;
    }
    /*
     * blank(spawner,destination) info를 stageData에 update
     */
    public void UpdateBlankInfo()
    {
        stageData.UpdateBlankInfo(blankList);
    }

    public void AddEnemyWaveInfoList(StageData.EnemyWaveInfo enemyWaveInfo)
    {
        enemyWaveInfoList.Add(enemyWaveInfo);
    }

    public void DeleteEnemyWaveInfoList(StageData.EnemyWaveInfo enemyWaveInfo)
    {
        enemyWaveInfoList.Remove(enemyWaveInfo);
    }

    /*
     * enemyWave info를 stageData에 update
     */
    public void UpdateEnemyWaveInfo()
    {
        stageData.UpdateEnemyWaveInfo(enemyWaveInfoList);
    }

    /*
     * Editor에 적용한 내용을 stageData에 update
     */
    public void UpdateStageData()
    {
        UpdateObstacleInfo();
        UpdateBlankInfo();
        UpdateEnemyWaveInfo();
    }

    /*
     * stage data를 json형식으로 저장
     */
    public void SaveStageData()
    {
        UpdateStageData();

        string jsonData = JsonUtility.ToJson(stageData);
        string path = string.Format("Assets/stageData/{0}.json", stageData.stageName);

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