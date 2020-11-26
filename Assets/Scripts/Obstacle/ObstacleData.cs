    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleData : MonoBehaviour
{
    public int lineNum;

    
    public class Neighbor
    {
        public GameObject obj;
        public Collider collider;
    }

    [HideInInspector]
    public List<Neighbor> NeighborList;

    private void Awake()
    {
        this.NeighborList = new List<Neighbor>();
    }
}
