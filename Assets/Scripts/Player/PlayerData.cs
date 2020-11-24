using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    public int currentCost;
    public int maxLife;
    public int currentLife;

    public void Init()
    {
        currentCost = 0;
        maxLife = 0;
        currentLife = 0;
    }

    public void SetPlayer(int cost, int life)
    {
        currentCost = cost;
        maxLife = life;
        currentLife = maxLife;
        // 컬러덱?
    }

    public bool CheckCost(int cost)
    {
        if (currentCost + cost < 0)
            return false;

        return true;
    }

    public bool ChangeCost(int cost)
    {
        
        if (currentCost + cost < 0)
            return false;

        currentCost += cost;

        return true;
    }

    public void ChangeLife(int life)
    {
        currentLife = Mathf.Clamp(currentLife + life, 0, maxLife);
        
        if (currentLife <= 0)
            Death();
    }

    public void Death()
    {
        GameManager.Instance.GameOver();
    }

}