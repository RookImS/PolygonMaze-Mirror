using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class StageSelectionUI : MonoBehaviour
{
    public GameObject menu;
    public string sceneToLoad;

    public void Deck(int i)
    {
        if (!GameManager.Instance.deckList[i].Contains(null))
        {
            GameManager.Instance.currentDeck = GameManager.Instance.deckList[i];
            for (int a = 0; a < 4; a++)
            {
                Debug.Log(GameManager.Instance.currentDeck[a].GetComponent<Skill>().id);
            }
            Debug.Log("스킬이 등록되었습니다.");
        }
        else
        {
            Debug.Log("스킬의 수가 부족하다.");
        }
    }

    public void LoadGame()
    {
        SceneManager.LoadScene(sceneToLoad);
        
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

                Debug.Log(GameManager.Instance.GetLoadStageChapter());
                Debug.Log(GameManager.Instance.GetLoadStageLevel());
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
        GameManager.Instance.SetLoadStageChapter(0);
        GameManager.Instance.SetLoadStageLevel(0);
        menu.SetActive(false);
    }
}
