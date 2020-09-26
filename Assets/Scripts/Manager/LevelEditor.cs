using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LevelEditor : MonoBehaviour
{
    private StageData stageData;
    private List<GameObject> obstacleList;
    private List<GameObject> spawnerList;
    private List<GameObject> destinationList;
    private List<StageData.EnemyWaveInfo> enemyWaveInfoList;

    public void Awake()
    {
        Init();
    }

    public void Init()
    {
        stageData = new StageData();
        obstacleList = new List<GameObject>();
        spawnerList = new List<GameObject>();
        destinationList = new List<GameObject>();
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

    public void AddSpawner(GameObject spawner)
    {
        spawnerList.Add(spawner);
    }

    public void DeleteSpawner(GameObject spawner)
    {
        spawnerList.Remove(spawner);
    }
    /*
     * spawner info를 stageData에 update
     */
    public void UpdateSpawnerInfo()
    {
        stageData.UpdateBlankInfo(spawnerList);
    }

    public void AddDestination(GameObject destination)
    {
        destinationList.Add(destination);
    }

    public void DeleteDestination(GameObject destination)
    {
        destinationList.Remove(destination);
    }

    /*
     * destination info를 stageData에 update
     */
    public void UpdateDestinationInfo()
    {
        stageData.UpdateBlankInfo(destinationList);
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
        UpdateSpawnerInfo();
        UpdateDestinationInfo();
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
        } catch (System.ArgumentException e1)
        {
            Debug.Log(e1.Message);
            // stage name 재입력 event
        } catch (System.Exception e2)
        {
            Debug.Log(e2.Message);
            // IOException or UnauthorizedAccessException
        }
    }
}


