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
    public int startMoney;

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

        public float positionX;
        public float positionY;
        public float positionZ;

        public float rotationW;
        public float rotationX;
        public float rotationY;
        public float rotationZ;
    }
    public List<ObstacleInfo> baseObstacles;

    [Serializable]
    public class BlankInfo
    {
        public enum BlankSpecific
        {
            Spawner,
            Destination
        }
        public BlankSpecific blankSpecific;

        public float positionX;
        public float positionY;
        public float positionZ;

        public float rotationW;
        public float rotationX;
        public float rotationY;
        public float rotationZ;
    }
    public List<BlankInfo> blankInfos;

    [Serializable]
    public class EnemyInfo
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
        public List<EnemyInfo> enemyInfos;
        public float enemySpawnTime;
        [Serializable]
        public enum NextPhaseTrigger
        {
            EnemyExterminated, // 적이 모두 등장
            FirstEnemyDead, // 해당 wave의 첫번째 적이 사망
            HalfOfEnemyDead, // 해당 wave의 절반의 적이 사망
            LastEnemyDead, // 해당 wave의 마지막 적이 사망
            Timer // 특정 시간이 지난 이후
        }
        public NextPhaseTrigger nextPhaseTrigger;
        public float timer;
    }
    public List<EnemyWaveInfo> baseEnemyWaveInfos;

    public StageData()
    {
        baseObstacles = new List<ObstacleInfo>();
        blankInfos = new List<BlankInfo>();
        baseEnemyWaveInfos = new List<EnemyWaveInfo>();
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

            obstacleInfo.positionX = obj.transform.position.x;
            obstacleInfo.positionY = obj.transform.position.y;
            obstacleInfo.positionZ = obj.transform.position.z;

            obstacleInfo.rotationW = obj.transform.rotation.w;
            obstacleInfo.rotationX = obj.transform.rotation.x;
            obstacleInfo.rotationY = obj.transform.rotation.y;
            obstacleInfo.rotationZ = obj.transform.rotation.z;
            baseObstacles.Add(obstacleInfo);
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

            if (temp.blank.tag == "Spawner")
                blankInfo.blankSpecific = BlankInfo.BlankSpecific.Spawner;
            else if (temp.blank.tag == "Destination")
                blankInfo.blankSpecific = BlankInfo.BlankSpecific.Destination;

            blankInfo.positionX = obj.transform.position.x;
            blankInfo.positionY = obj.transform.position.y;
            blankInfo.positionZ = obj.transform.position.z;

            blankInfo.rotationW = obj.transform.rotation.w;
            blankInfo.rotationX = obj.transform.rotation.x;
            blankInfo.rotationY = obj.transform.rotation.y;
            blankInfo.rotationZ = obj.transform.rotation.z;
            blankInfos.Add(blankInfo);
        }
    }

    public void UpdateEnemyWaveInfo(List<EnemyWaveInfo> enemyWaveInfoList)
    {
        //EnemyWaveInfo enemyWaveInfo;
        foreach (EnemyWaveInfo obj in enemyWaveInfoList)
        {
            baseEnemyWaveInfos.Add(obj);

            /*
            enemyWaveInfo = new EnemyWaveInfo();
            enemyWaveInfo.enemyInfos = obj.enemyInfos;
            enemyWaveInfo.enemySpawnTime = obj.enemySpawnTime;
            enemyWaveInfo.nextPhaseTrigger = obj.nextPhaseTrigger;
            enemyWaveInfo.timer = obj.timer;
            */

        }
    }
}
