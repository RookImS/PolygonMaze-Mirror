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
    [HideInInspector] public TowerSkill towerSkill;

    public virtual void Init()
    {
        Stats.Init(this);
        towerSkill = null;
    }

    public virtual void Shoot(Transform muzzle)
    {
    }

    public void ApplyTowerSkill(TowerSkill towerSkill)
    {
        this.towerSkill = towerSkill;
    }
}