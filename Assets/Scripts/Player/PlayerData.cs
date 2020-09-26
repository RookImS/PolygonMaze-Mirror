using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public int cost;
    public int life;

    public bool Init()
    {

        this.cost = 1000;
        this.life = 10;
        // 파일 정보 불러오기
        return true;
    }
    public void SetPlayer(int cost, int life)
    {
        this.cost = cost;
        this.life = life;

        // 컬러덱?
    }


    public bool ChangeCost(int cost)
    {
        if (this.cost + cost < 0)
            return false;

        this.cost += cost;
        return true;
    }


    public void ChangeLife(int life)
    {
        this.life += life;
    }


    public void Death()
    {
        // 죽었을 시 걸리는 트리거
    }

}