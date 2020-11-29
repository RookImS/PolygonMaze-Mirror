using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;

public class StageData
{
    public int stageChapter;
    public int stageLevel;

    public int playerLife;
    public int startCost;


    [Serializable]
    public class BlankInfo
    {
        public enum BlankMeshType
        {
            Mesh1,
            Mesh2,
            Mesh3
        }
        public BlankMeshType blankMeshType;

        public Vector3 position;
        public Quaternion rotation;
    }

    public BlankInfo spawnerInfo;
    public BlankInfo destinationInfo;


    [Serializable]
    public class ObstacleInfo
    {
        public enum ObstacleSpecific
        {
            Triangle,
            Square,
            Pentagon,
            Hexagon
        }
        public ObstacleSpecific obstacleSpecific;

        public Vector3 position;
        public Quaternion rotation;
    }
    public List<ObstacleInfo> obstacles;


    [Serializable]
    public class Enemies
    {
        public enum EnemySpecific
        {
            Circle,
            SemiCircle,
            Sector,
            Ellipse,
            Ring
        }
        public EnemySpecific enemySpecific;
        public int count; 
    }

    [Serializable]
    public class EnemyWaveInfo
    {
        public List<Enemies> enemyOneWave;
        public float enemySpawnDuration;
        [Serializable]
        public enum NextWaveTrigger
        {
            EnemyExterminated, // 적이 모두 등장
            FirstEnemyDead, // 해당 wave의 첫번째 적이 사망
            HalfOfEnemyDead, // 해당 wave의 절반의 적이 사망
            LastEnemyDead, // 해당 wave의 마지막 적이 사망
        }
        public NextWaveTrigger nextWaveTrigger;

        public float breakTime;
    }

    public List<EnemyWaveInfo> enemyWaveInfos;
    public int enenmyNum;

    [Serializable]
    public class ReadyFlag
    {
        public bool playerSetting = false;
        public bool spawnerSetting = false;
        public bool destinationSetting = false;
        public bool obstacleSetting = false;
        public bool enemyWaveSetting = false;
        public bool saveSetting = false;
        public bool loadSetting = false;
    }

    public ReadyFlag readyFlag;

    public StageData()
    {
        stageChapter = 0;
        stageLevel = 0;
        playerLife = 0;
        startCost = 0;
        obstacles = new List<ObstacleInfo>();
        spawnerInfo = null;
        destinationInfo = null;
        enemyWaveInfos = new List<EnemyWaveInfo>();
        enenmyNum = 0;
        readyFlag = new ReadyFlag();
    }

    public void UpdatePlayerInfo(int playerLife, int startCost)
    {
        this.playerLife = playerLife;
        this.startCost = startCost;
    }

    public void UpdateStageChapterAndLevel(int stageChapter, int stageLevel)
    {
        this.stageChapter = stageChapter;
        this.stageLevel = stageLevel;
    }

    public void UpdateBlankInfo(LevelEditor.BlankTempInfo blankTempInfo)
    {
        GameObject obj;
        BlankInfo blankInfo;

        if (blankTempInfo != null)
        {
            obj = blankTempInfo.blank;
            blankInfo = new BlankInfo();

            blankInfo.blankMeshType = blankTempInfo.blankMeshType;

            blankInfo.position = obj.transform.position;
            blankInfo.rotation = obj.transform.rotation;

            if (blankTempInfo.blank.tag == "Spawner")
                spawnerInfo = blankInfo;
            else if (blankTempInfo.blank.tag == "Destination")
                destinationInfo = blankInfo;
        }

    }

    public void UpdateObstacleInfo(List<GameObject> gameObjectList)
    {
        ObstacleInfo obstacleInfo;
        foreach (GameObject obj in gameObjectList)
        {
            obstacleInfo = new ObstacleInfo();

            if (obj.GetComponent<ObstacleData>().lineNum == 3)
                obstacleInfo.obstacleSpecific = ObstacleInfo.ObstacleSpecific.Triangle;
            else if (obj.GetComponent<ObstacleData>().lineNum == 4)
                obstacleInfo.obstacleSpecific = ObstacleInfo.ObstacleSpecific.Square;
            else if (obj.GetComponent<ObstacleData>().lineNum == 5)
                obstacleInfo.obstacleSpecific = ObstacleInfo.ObstacleSpecific.Pentagon;
            else
                obstacleInfo.obstacleSpecific = ObstacleInfo.ObstacleSpecific.Hexagon;

            obstacleInfo.position = obj.transform.position;
            obstacleInfo.rotation = obj.transform.rotation;

            obstacles.Add(obstacleInfo);
        }
    }

    public void SortObstacleInfo()
    {
        obstacles = obstacles.OrderBy(x => x.position.x).ThenBy(x => x.position.z).ToList<ObstacleInfo>();
    }

    public void UpdateEnemyWaveInfo(List<EnemyWaveInfo> enemyWaveInfoList)
    {
        foreach (EnemyWaveInfo obj in enemyWaveInfoList)
        {
            enemyWaveInfos.Add(obj);
        }
    }

    public void UpdateEnemyNum(int enemyNum)
    {
        this.enenmyNum = enemyNum;
    }

    public void UpdateReadyFlag(ReadyFlag readyFlag)
    {
        this.readyFlag.playerSetting = readyFlag.playerSetting;
        this.readyFlag.spawnerSetting = readyFlag.spawnerSetting;
        this.readyFlag.destinationSetting = readyFlag.destinationSetting;
        this.readyFlag.obstacleSetting = readyFlag.obstacleSetting;
        this.readyFlag.enemyWaveSetting = readyFlag.enemyWaveSetting;
        this.readyFlag.saveSetting = readyFlag.saveSetting;
    }
}
