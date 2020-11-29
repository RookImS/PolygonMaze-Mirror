using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleBehaviour : MonoBehaviour
{
    private ObstacleData m_ObstacleData;

    private void Awake()
    {
        m_ObstacleData = GetComponent<ObstacleData>();
    }
    private void OnMouseDown()
    {
        LevelEditorUISystem.instance.GetObstacleDeleteUISystem().OnClickObstacle(this.gameObject);
    }

    public void SetNeighbor(ObstacleData.Neighbor neighbor)
    {
        m_ObstacleData.NeighborList.Add(neighbor);
    }
}
