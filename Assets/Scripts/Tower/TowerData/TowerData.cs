using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerData : MonoBehaviour
{
    public string TowerName;
    public string TowerDescription;

    public TowerStatSystem Stats;
    public int cost;
    public List<GameObject> neighbor;
    
    public TColor color;    // public x
    public GameObject bullet;

    public void Init()
    {
        Stats.Init(this);
    }

    public virtual void Shoot(Transform muzzle)
    {
    }
}
