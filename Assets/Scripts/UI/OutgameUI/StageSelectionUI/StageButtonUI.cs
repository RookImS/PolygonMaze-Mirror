using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageButtonUI : MonoBehaviour
{
    public int a;
    public int b;
    public bool isStageClear;

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
            gameObject.GetComponent<Button>().image.color = Color.red;
        }
    }

}
