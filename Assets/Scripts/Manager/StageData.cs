using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class StageData
{
    public string stageName;
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
        public enum NextPhaseTrigger
        {
            EnemyExterminated, // 적이 모두 등장
            FirstEnemyDead, // 해당 wave의 첫번째 적이 사망
            HalfOfEnemyDead, // 해당 wave의 절반의 적이 사망
            LastEnemyDead, // 해당 wave의 마지막 적이 사망
        }
        public NextPhaseTrigger nextPhaseTrigger;

        public float breakTime;
    }

    public List<EnemyWaveInfo> enemyWaveInfos;
    public int enenmyNum;

    public StageData()
    {
        obstacles = new List<ObstacleInfo>();
        spawnerInfo = null;
        destinationInfo = null;
        enemyWaveInfos = new List<EnemyWaveInfo>();
    }

    public void UpdateObstacleInfo(List<GameObject> gameObjectList)
    {
        ObstacleInfo obstacleInfo;
        foreach (GameObject obj in gameObjectList)
        {
            obstacleInfo = new ObstacleInfo();
            if (obj.GetComponent<ObstacleData>().lineNum == 3)
                obstacleInfo.obstacleSpecific = ObstacleInfo.ObstacleSpecific.Triangle;
            else if(obj.GetComponent<ObstacleData>().lineNum == 4)
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

    public void UpdateBlankInfo(List<LevelEditor.BlankTempInfo> blankTempInfoList)
    {
        GameObject obj;
        BlankInfo blankInfo;

        foreach (LevelEditor.BlankTempInfo temp in blankTempInfoList)
        {
            obj = temp.blank;
            blankInfo = new BlankInfo();

            blankInfo.blankMeshType = temp.blankMeshType;

            blankInfo.position = obj.transform.position;
            blankInfo.rotation = obj.transform.rotation;

            if (temp.blank.tag == "Spawner")
                spawnerInfo = blankInfo;
            else if (temp.blank.tag == "Destination")
                destinationInfo = blankInfo;
        }
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
}
