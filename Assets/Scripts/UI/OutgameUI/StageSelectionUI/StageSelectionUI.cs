﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class StageSelectionUI : MonoBehaviour
{
    public GameObject menu;
    public string sceneToLoad;

    private bool isSelectDeck;

    private void Awake()
    {
        Init();
  
    }

    private void Init()
    {
        isSelectDeck = false;

        GameManager.Instance.SetLoadStageChapter(0);
        GameManager.Instance.SetLoadStageLevel(0);
    }

    public void SelectDeck(int num)
    {
        bool isNull = false;

        for(int i = 0; i< GameManager.Instance.deckList[num].Count; i++)
        {
            if (GameManager.Instance.deckList[num][i] == null)
            {
                isNull = true;
                break;
            }
        }

        if (!isNull)
        {
            GameManager.Instance.currentDeck = GameManager.Instance.deckList[num];

            isSelectDeck = true;

            for(int i = 0; i < GameManager.Instance.deckList[num].Count; i++)
            {
                Debug.Log(i + ": " + GameManager.Instance.deckList[num][i]);
            }
            Debug.Log("스킬이 등록되었습니다.");
        }
        else
        {
            isSelectDeck = false;
            Debug.Log("스킬의 수가 부족하다.");
        }
    }

    public void LoadGame()
    {
        if (isSelectDeck)
            SceneManager.LoadScene(sceneToLoad);
        else
            Debug.Log("Deck 없어!");
    }

    public void OnClickStageButton(string stage)
    {
        string[] chapterAndLevel = stage.Split('-');
        int chapter = 0;
        int level = 0;

        if(chapterAndLevel.Length != 2)
        {
            Debug.Log("OnClickStageButton evenet setting error.");
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
    }

    public void OnClickCancleButton()
    {
        Init();
        menu.SetActive(false);
    }
}