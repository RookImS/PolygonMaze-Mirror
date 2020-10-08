using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    public int cost;
    public int life;
    public int currentLife;

    public bool Init()
    {
        this.cost = 500;
        this.life = 50;
        this.currentLife = this.life;

        // 파일 정보 불러오기

        return true;
    }
    public void SetPlayer(int cost, int life)
    {
        this.cost = cost;
        this.life = life;
        this.currentLife = this.life;

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
        this.currentLife = Mathf.Clamp(this.currentLife + life, 0, this.life);
        if (currentLife <= 0)
            Death();
    }


    public void Death()
    {
        Debug.Log("player death");
        // 죽었을 시 걸리는 트리거
    }

}