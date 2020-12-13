using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageButtonUI : MonoBehaviour
{
    public int a;
    public int b;
    public bool isStageClear;
    public GameObject clearFlag;
    public Image stageArrow;
    void Start()
    {
        Init();
        StageClearCheck();
    }

    void Init()
    {
        isStageClear = GameManager.Instance.stageClearInfo.stageChapter[a].stageLevel[b].isClear;
    }
    public void StageClearCheck()
    {
        if (isStageClear)
        {
            clearFlag.SetActive(true);
            stageArrow.color = new Color(0, 130f/255f, 0, 1);
            if (!GameManager.Instance.stageClearInfo.stageChapter[a].stageLevel[b + 1].isClear)
            {
                stageArrow.color = Color.yellow;
            }
        }
        
    }

}
