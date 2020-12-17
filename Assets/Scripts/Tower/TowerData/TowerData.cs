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

    protected GameObject bulletObject;

    private void Update()
    {
        Stats.Tick();
    }

    public virtual void Init()
    {
        Stats.Init(this);
        bulletObject = GameObject.Find("Bullet");
    }

    public virtual void Shoot(Transform muzzle)
    {
    }

    public virtual void SetSkillEffect(GameObject effect)
    {
        Instantiate(effect, transform);
    }

    public void RemoveSkillEffect(GameObject effect)
    {
        GameObject targetEffect = transform.Find(effect.name + "(Clone)").gameObject;
        Destroy(targetEffect);
    }
}