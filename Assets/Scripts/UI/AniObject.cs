using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AniObject
{
    public AniObject (AniObject a)
    {
        order = a.order;
        length = a.length;
        posX = a.posX;
        posY = a.posY;
        rotation = a.rotation;
        obj = a.obj;
        enable = a.enable;
    }

    [Header("애니메이션 오브젝트")]
    public GameObject obj;

    [Header("챕터 내 생성시기 및 유지길이")]
    [SerializeField]
    public int order;
    public int length = 1;

    [Header("생성 위치(애니메이션 도착 기준)")]
    [SerializeField]
    public float posX;
    public float posY;
    public float rotation;

    [HideInInspector]public bool enable = false;
}
