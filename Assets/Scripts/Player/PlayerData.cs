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
        this.cost = 99999;
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

        Debug.Log(string.Format("player current cost : {0}", this.cost));
        return true;
    }

    public void ChangeLife(int life)
    {
        this.currentLife = Mathf.Clamp(this.currentLife + life, 0, this.life);
        Debug.Log(string.Format("Player current life : {0}", this.currentLife));
        if (currentLife <= 0)
            Death();
    }


    public void Death()
    {
        Debug.Log("player death");
        // 죽었을 시 걸리는 트리거
    }

}