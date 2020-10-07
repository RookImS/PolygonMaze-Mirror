using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Stage00", menuName ="ScriptableObjects/StageScriptableObject", order = 1 )]
public class StageScriptableObject : ScriptableObject
{
    public string stageName;
    public float stageLevel;

    public int startCost;
    public int playerLife;
    
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

    [Header("기본 장애물 정보")]
    public List<ObstacleInfo> baseObstacles;

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
    [Header("기본 Spawner 정보")]
    public BlankInfo spawnerInfo;

    [Header("기본 Destination 정보")]
    public BlankInfo destinationInfo;

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
    public class EnemyPhaseInfo
    {
        [Header("1페이즈 당 적 정보")]
        public List<EnemyInfo> enemyInfoList;

        [Header("적 생성 주기")]
        public float enemySpawnDuration = 0.5f;
        [Serializable]
        public enum NextPhaseTrigger
        {
            EnemyExterminated, // 적이 모두 등장
            FirstEnemyDead, // 해당 wave의 첫번째 적이 사망
            HalfOfEnemyDead, // 해당 wave의 절반의 적이 사망
            LastEnemyDead, // 해당 wave의 마지막 적이 사망
            Timer // 특정 시간(timer)이 지난 이후
        }
        [Header("다음 페이즈 실행 조건")]
        public NextPhaseTrigger nextPhaseTrigger;

        [Header("페이즈 실행 조건이 Timer일 때 Setting")]
        public int timer = 0;

        [Header("다음 페이즈 실행 전까지 휴식 시간")]
        public float breakTime = 5.0f;
    }

    [Header("적 페이즈 정보")]
    public List<EnemyPhaseInfo> baseEnemyPhases;


}
