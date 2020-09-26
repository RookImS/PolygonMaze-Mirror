using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.Match;

public class ExampleLevelManager : MonoBehaviour
{


    public StageScriptableObject stageData;

    // Start is called before the first frame update
    void Start()
    {
        if(stageData != null)
        {
            Debug.Log("Stage Name : " + stageData.stageName);
            Debug.Log("Stage Start Money : " + stageData.startMoney);
            Debug.Log("Stage Level : " + stageData.stageLevel);


            Debug.Log("-----------------------------");
            Debug.Log("Stage Base Obstacles Count : " + stageData.baseObstacles.Count);

            if (stageData.baseObstacles.Count > 0)
            {
                GameObject obstacleParet = new GameObject("Obstacles");
                obstacleParet.transform.parent = transform;

                foreach(StageScriptableObject.ObstacleInfo info in stageData.baseObstacles)
                {
                    GameObject obj = Instantiate(info.prefab);
                    obj.transform.position = info.position;
                    obj.transform.parent = obstacleParet.transform;
                }

            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
