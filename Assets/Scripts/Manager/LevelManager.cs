using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance { get; private set; }

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


    private void Awake()
    {
        instance = this;
        PlayerControl.Instance.playerData.SetPlayer(this.stageData.startCost, this.stageData.playerLife);
        LoadStage();
    }

    /* LoadStage
     * 1. Load obstacles.
     * 2. Load spawner and destination & Set active of overlabed wall to false.
     * 3. Bake enemy navmesh surface and checker navmesh surface.
     */
    public void LoadStage()
    {
        LoadObstacles();
        LoadBlank();
        BakeNavMeshSurfaces();
        LoadChecker();
    }

    /* LoadObstacles
     * 1. Load obstacles.
     * 2. Etc..
     */
    public void LoadObstacles()
    {
        GameObject obstacles = null;
        GameObject prefab = null;


        obstacles = new GameObject("Obstacles");
        obstacles.transform.SetParent(this.stageGameObject.transform);

        foreach (StageScriptableObject.ObstacleInfo info in this.stageData.baseObstacles)
        {
            if (StageScriptableObject.ObstacleInfo.ObstacleSpecific.Triangle
                == info.obstacleSpecific)
                prefab = this.triangleObstaclePrefab;
            else if (StageScriptableObject.ObstacleInfo.ObstacleSpecific.Square
                == info.obstacleSpecific)
                prefab = this.squareObstaclePrefab;
            else if (StageScriptableObject.ObstacleInfo.ObstacleSpecific.Pentagon
                == info.obstacleSpecific)
                prefab = this.pentagonObstaclePrefab;
            else // Hexagon
                prefab = this.hexagonObstaclePrefab;

            GameObject.Instantiate(prefab, info.position, info.rotation, obstacles.transform);
        }
    }

    /* LoadBlank
     * 1. Load spawner and destination
     * 2. Set active of overlabed wall to false.
     * 3. Etc..
     */
    public void LoadBlank()
    {
        StageScriptableObject.BlankInfo info = null; 
        GameObject prefab = null;
        GameObject spawner = null;
        GameObject destination = null;
        

        //Load Spawner
        info = this.stageData.spawnerInfo;
        prefab = this.spawnerPrefab;
        spawner = GameObject.Instantiate(prefab, info.position, info.rotation, this.stageGameObject.transform);
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
     * 1. Set active of a appropriate mesh to true
     * 2. Set active of a appropriate mesh to false
     */
    public void SelectBlankMesh(GameObject blank, StageScriptableObject.BlankInfo blankInfo)
    {
        if (StageScriptableObject.BlankInfo.BlankMeshType.Mesh1
            == blankInfo.blankMeshType)
        {
            blank.transform.GetChild(0).gameObject.SetActive(true);
            blank.transform.GetChild(1).gameObject.SetActive(false);
            blank.transform.GetChild(2).gameObject.SetActive(false);
        }
        else if (StageScriptableObject.BlankInfo.BlankMeshType.Mesh1
            == blankInfo.blankMeshType)
        {
            blank.transform.GetChild(0).gameObject.SetActive(false);
            blank.transform.GetChild(1).gameObject.SetActive(true);
            blank.transform.GetChild(2).gameObject.SetActive(false);
        }
        else
        {
            blank.transform.GetChild(0).gameObject.SetActive(false);
            blank.transform.GetChild(1).gameObject.SetActive(false);
            blank.transform.GetChild(2).gameObject.SetActive(true);
        }
    }

    /* SetWallActiveToFalse
     * 1. Check if the wall overlabs with blank(spawner or destination)
     * 2. Set active of wall to false if overlabed
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
            foreach(Transform wallTs in wallLineTs.gameObject.GetComponentInChildren<Transform>())
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

    public void LoadChecker()
    {
        StageScriptableObject.BlankInfo info = null;

        info = this.stageData.spawnerInfo;
        GameObject.Instantiate(this.checkerPrefab, info.position, info.rotation, this.stageGameObject.transform);  
    }

}
