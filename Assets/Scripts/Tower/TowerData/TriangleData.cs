﻿using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriangleData : TowerData
{
    public int bulletNumber;
    public float bulletAngle;

    public override void Shoot(Transform muzzle)
    {
        List<GameObject> bulletInstanceList = new List<GameObject>();

        float eachBulletAngle = bulletAngle / (bulletNumber-1);
        Vector3 rotateAngle = new Vector3(0f, eachBulletAngle, 0f);

        muzzle.Rotate(new Vector3(0f, -(bulletAngle / 2), 0f));
        for (int i = 0; i < bulletNumber; i++)
        {
            bulletInstanceList.Add(Instantiate(bullet, muzzle.position, muzzle.rotation));
            muzzle.Rotate(rotateAngle);
        }

        foreach(GameObject bulletInstance in bulletInstanceList)
            bulletInstance.GetComponent<BulletBehaviour>().Init(Stats, towerSkill);
    }
}