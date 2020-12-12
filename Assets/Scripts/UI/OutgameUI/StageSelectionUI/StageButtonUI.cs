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
        clearFlag.SetActive(isStageClear);
    }

}
