using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBehaviour : MonoBehaviour
{
    TowerData m_TowerData;

    void Awake()
    {
        m_TowerData = GetComponent<TowerData>();
    }

    public void setNeighbor(GameObject obj)
    {
        m_TowerData.neighbor.Add(obj);
    }

    
}
