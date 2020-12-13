using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DeckUI : MonoBehaviour, IPointerDownHandler
{
    public SkillListUI skillListUI;
    public int order;
    public Toggle flagToggle;
    public GameObject thisDeck;
    public GameObject SkillList;
    public ToggleGroup flagGroup;

    private List<GameObject> deck;
    private bool isClicked;
    private bool isSkill;
    private bool isFirst;
    private float clickTime;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        deck = GameManager.Instance.deckList[order];

        isFirst = false;
        if (System.Object.ReferenceEquals(deck, GameManager.Instance.currentDeck) && flagToggle.interactable)
        {
            isFirst = true;
            flagToggle.isOn = true;
        }
        isFirst = false;

        isClicked = false;
        isSkill = false;
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
    public void PointerDown(int num)
    {
        isSkill = true;
        StartCoroutine(CallDeleteSkill(num));
    }
    public void PointerUp(int num)
    {
        StopAllCoroutines();
        if (isSkill && !(deck[num] == null))
        {
            skillListUI.SelectSkill(deck[num]);
        }
        isSkill = false;
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

        if (!isFirst && flagToggle.isOn && SoundManager.Instance != null)
            SoundManager.instance.PlaySound(SoundManager.SoundSpecific.BUTTON, "Deck_Select_SE");
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
   
            GameManager.Instance.SaveDeckInfos(order);

            if (SoundManager.Instance != null)
                SoundManager.instance.PlaySound(SoundManager.SoundSpecific.BUTTON, "Common_Button");
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
    IEnumerator CallDeleteSkill(int num)
    {
        while (true)
        {
            if (isSkill)
            {
                clickTime += Time.deltaTime;
                yield return null;
                if (skillListUI.insertCheck)
                {
                    InsertDeck(num);
                    isSkill = false;
                    break;
                }
            }

            if (clickTime >= 0.5f)
            {
                deck[num] = null;
                thisDeck.transform.GetChild(num).Find("SkillImage").GetComponent<Image>().color = Color.white;
                isSkill = false;
                if (SoundManager.instance != null)
                    SoundManager.instance.PlaySound(SoundManager.SoundSpecific.BUTTON, "Button_Fail");
                break;
            }
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

        if (SoundManager.instance != null)
            SoundManager.instance.PlaySound(SoundManager.SoundSpecific.BUTTON, "Deck_Click");
    }
}
