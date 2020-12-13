using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class StatusUI : MonoBehaviour
{
    private int preCost;
    private int currentCost;
    public TextMeshProUGUI costText;

    private int preLife;
    private int currentLife;
    public TextMeshProUGUI lifeText;

    private int preEnemyCount;
    private int currentEnemyCount;
    public TextMeshProUGUI enemyCountText;

    private void Awake()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        if(!GameManager.Instance.isStageClear)
            UpdateStatus();
    }

    private void Init()
    {
        preCost = 0;
        preLife = 0;
        currentCost = 0;
        currentLife = 0;
    }

    void UpdateStatus()
    {
        currentCost = PlayerControl.Instance.playerData.currentCost;
        currentLife = PlayerControl.Instance.playerData.currentLife;

        if(LevelManager.instance.stageData != null)
            currentEnemyCount = LevelManager.instance.UpdateEnemyCount();

        if (preCost != currentCost)
            costText.text = PlayerControl.Instance.playerData.currentCost.ToString();
        if (preLife != currentLife)
            lifeText.text = PlayerControl.Instance.playerData.currentLife.ToString();
        if (preEnemyCount != currentEnemyCount)
            enemyCountText.text = LevelManager.instance.m_enemyCount.ToString();

        preCost = currentCost;
        preLife = currentLife;
        preEnemyCount = currentEnemyCount;
    }
}
