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

    private void Awake()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
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
        currentCost = PlayerControl.Instance.playerData.cost;
        currentLife = PlayerControl.Instance.playerData.life;

        if (preCost != currentCost)
            costText.text = PlayerControl.Instance.playerData.cost.ToString();
        if (preLife != currentLife)
            lifeText.text = PlayerControl.Instance.playerData.life.ToString();

        preCost = currentCost;
        preLife = currentLife;
    }
}
