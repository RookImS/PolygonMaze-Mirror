using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageButtonUI : MonoBehaviour
{
    public int chapter;
    public int level;
    public GameObject clearFlag;
    public Image stageArrow;
    void Start()
    {
        StageClearCheck();
    }


    public void StageClearCheck()
    {
        if (!this.GetComponent<Button>().interactable)
        {
            if (GameManager.Instance.stageClearInfo.stageChapter[chapter - 1].stageLevel[level - 2].isClear)
            {
                this.GetComponent<Button>().interactable = true;
            }
        }
        if (GameManager.Instance.stageClearInfo.stageChapter[chapter - 1].stageLevel[level - 1].isClear)
        {
            clearFlag.SetActive(true);
            if(stageArrow != null)
            {
                stageArrow.color = new Color(0, 130f / 255f, 0, 1);
                if (!GameManager.Instance.stageClearInfo.stageChapter[chapter - 1].stageLevel[level].isClear)
                {
                    stageArrow.color = Color.yellow;
                }
            }

        }

    }

}
