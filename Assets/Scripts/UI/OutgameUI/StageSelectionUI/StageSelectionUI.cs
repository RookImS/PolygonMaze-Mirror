using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System;

public class StageSelectionUI : MonoBehaviour
{
    public GameObject menu;
    public string sceneToLoad;
    public TextMeshProUGUI stageNum;
    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        GameManager.Instance.SetLoadStageChapter(0);
        GameManager.Instance.SetLoadStageLevel(0);

    }

    public void LoadGame()
    {
        if (GameManager.Instance.currentDeck != null)
        {
            SoundManager.Instance.PlayBGM("InGame_BGM");
            SceneManager.LoadScene(sceneToLoad);
            SoundManager.Instance.PlaySound(SoundManager.SoundSpecific.BUTTON, "Confirm_Button");
        }
        else
        {
            Debug.Log("Deck 없어!");
            SoundManager.Instance.PlaySound(SoundManager.SoundSpecific.BUTTON, "Button_Fail");
        }
    }

    public void OnClickStageButton(string stage)
    {
        string[] chapterAndLevel = stage.Split('-');
        stageNum.text = "Stage <color=black>"+stage+"</color> 에\n도전하시겠습니까?";
        stageNum.text.Replace("\\n", "\n");
        int chapter = 0;
        int level = 0;

        if(chapterAndLevel.Length != 2)
        {
            Debug.Log("OnClickStageButton event setting error.");
        }
        else         
        {
            if (int.TryParse(chapterAndLevel[0], out chapter)
                && int.TryParse(chapterAndLevel[1], out level))
            {
                GameManager.Instance.SetLoadStageChapter(chapter);
                GameManager.Instance.SetLoadStageLevel(level);
            }
            else
            {
                Debug.Log("OnClickStageButton evenet setting error.");
            }
        }

        menu.SetActive(true);
        SoundManager.Instance.PlaySound(SoundManager.SoundSpecific.BUTTON, "Common_Button");
    }

    public void OnClickCancleButton()
    {
        Init();
        menu.SetActive(false);
        SoundManager.Instance.PlaySound(SoundManager.SoundSpecific.BUTTON, "Cancle_Button");
    }
}
