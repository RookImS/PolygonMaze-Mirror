using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoSingleton<PlayerControl>
{
    public PlayerData playerData;

    private void Awake()
    {
        playerData = new PlayerData();
        Init();
    }

    public void Init()
    {
        playerData.Init();
    }

    public void SetPlayer(int cost, int life)
    {
        playerData.SetPlayer(cost, life);
    }

    public void GainCost(int reward)
    {
        playerData.ChangeCost(reward);
    }

    public bool UseCost(int cost)
    {
        return playerData.ChangeCost(-cost);
    }

    public void Damage(int life)
    {
        playerData.ChangeLife(-life);
    }

    public void Call()
    {
        Debug.Log("remain cost : " + playerData.cost + " // remain life : " + playerData.life);
    }
}

