using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DeckUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public SkillListUI skillListUI;
    public int order;
    public Toggle flagToggle;
    public GameObject thisDeck;
    public GameObject SkillList;
    public ToggleGroup flagGroup;

    private List<GameObject> deck;
    private bool isClicked;
    private float clickTime;

    private void Start()
    {
        Init();
    }
    private void Init()
    {
        deck = GameManager.Instance.deckList[order];

        if (deck == GameManager.Instance.currentDeck)
        {
            flagToggle.isOn = true;
        }

        isClicked = false;
        clickTime = 0;
        DeckCheck(order);

        for (int i = 0; i < deck.Count; i++)
        {
            if (deck[i] == null)
                thisDeck.transform.GetChild(i).Find("SkillImage").GetComponent<Image>().color = Color.white;
            else
                thisDeck.transform.GetChild(i).Find("SkillImage").GetComponent<Image>().color = deck[i].GetComponent<Skill>().color;
        }
    }
    public void OnPointerDown(PointerEventData pointerEventData)
    {
        isClicked = true;
        StartCoroutine(CheckCallSkillList());
    }

    public void OnPointerUp(PointerEventData pointerEventData)
    {
        StopAllCoroutines();
        isClicked = false;
        clickTime = 0;
    }

    public void ToggleDeck()
    {
        GameManager.Instance.currentDeck = GameManager.Instance.deckList[order];
        for (int i = 0; i < GameManager.instance.allDeckInfo.deckInfoList.Count; i++)
        {
            if (i == order)
                GameManager.instance.allDeckInfo.deckInfoList[i].isCurrent = true;
            else
                GameManager.instance.allDeckInfo.deckInfoList[i].isCurrent = false;
        }
    }

    public void InsertDeck(int num)
    {
        if (skillListUI.insertCheck)
        {
            //GameObject selectSlotSkill = deck[num];

            for (int i = 0; i < deck.Count; i++)
            {
                if (i == num)
                    continue;
                if (deck[i] == skillListUI.selectedSkill)
                {
                    if (deck[num] == null)
                    {
                        deck[i] = null;
                        thisDeck.transform.GetChild(i).Find("SkillImage").GetComponent<Image>().color = Color.white;
                    }
                    else
                    {
                        deck[i] = deck[num];
                        thisDeck.transform.GetChild(i).Find("SkillImage").GetComponent<Image>().color = deck[num].GetComponent<Skill>().color;
                    }

                }
            }
            deck[num] = skillListUI.selectedSkill;
            thisDeck.transform.GetChild(num).Find("SkillImage").GetComponent<Image>().color = skillListUI.selectedSkill.GetComponent<Skill>().color;

            DeckCheck(order);
            skillListUI.insertCheck = false;
            skillListUI.skillPanel.SetActive(false);
            GameManager.Instance.SaveDeckInfos(order);
        }
        else
            return;
    }

    public void DeckCheck(int num)
    {
        bool isNull = false;
        for (int i = 0; i < GameManager.Instance.deckList[num].Count; i++)
        {
            if (GameManager.Instance.deckList[num][i] == null)
            {
                isNull = true;
                break;
            }
        }

        if (!isNull)
        {
            flagToggle.interactable = true;
            GetComponent<CanvasGroup>().alpha = 1f;
            //Debug.Log("덱이 완성되었다.");
        }
        else
        {
            flagToggle.interactable = false;
        }
    }
    private IEnumerator CheckCallSkillList()
    {
        while (true)
        {
            if (isClicked)
            {
                clickTime += Time.deltaTime;
                yield return null;
            }
            else
            {
                Init();
                yield return null;
            }
            if (clickTime >= 0.5f)
                break;
        }

        flagToggle.interactable = false;
        SkillList.SetActive(true);
        skillListUI.HighlightDeck(order);
    }
}
