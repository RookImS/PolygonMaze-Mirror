using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Stage00", menuName ="ScriptableObjects/StageScriptableObject", order =1 )]
public class StageScriptableObject : ScriptableObject
{
    public string stageName = "";
    public float stageLevel = 1f;
    public float startMoney = 200f;


    [Serializable]
    public class ObstacleInfo
    {
        public Vector3 position;
        public GameObject prefab;
    }
    [Header("기본 장애물 정보")]
    //[Serializable]
    public List<ObstacleInfo> baseObstacles;

    [Serializable]
    public class EnemyPhaseData
    {
        public GameObject prefab;
        public int count; 
    }
    [Serializable]
    public class EnemyPhaseInfo
    {
        [Header("페이즈 정보")]
        public List<EnemyPhaseData> phaseDatas;
        [Header("페이즈 지속 시간")]
        public float enemySpawnTime = 10f;
        [Serializable]
        public enum NextPhaseTrigger
        {
            EnemyExterminated,
            HalfOfEnemyDead,
            FirstEnemyDead,
            LastEnemyDead,
            Timer
        }
        [Header("다음 페이즈 실행 조건")]
        public NextPhaseTrigger nextPhaseTrigger;
        public int timer = 0;
    }
    [Header("적 페이즈 정보")]
    public List<EnemyPhaseInfo> baseEnemyPhases;


}
