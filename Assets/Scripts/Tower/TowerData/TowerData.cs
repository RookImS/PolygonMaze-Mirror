using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerData : MonoBehaviour
{
    public string TowerName;
    public string TowerDescription;

    public TowerStatSystem Stats;
    public int cost;
    public List<GameObject> neighbor;   // public x?
    public bool isDeploy;   // public x

    public TColor color;    // public x
    
}
