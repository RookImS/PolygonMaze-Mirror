using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerData : MonoBehaviour
{
    public string TowerName;
    public string TowerDescription;
    public GameObject bullet;
    public TowerStatSystem Stats;

    [HideInInspector] public List<GameObject> neighbor;

    private void Update()
    {
        Stats.Tick();
    }

    public virtual void Init()
    {
        Stats.Init(this);
    }

    public virtual void Shoot(Transform muzzle)
    {
    }
}