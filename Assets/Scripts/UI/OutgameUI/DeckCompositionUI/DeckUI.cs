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

    private void Awake()
    {
        Init();
        
    }
    private void Init()
    {
        deck = GameManager.Instance.deckList[order];

        isClicked = false;
        clickTime = 0;
        DeckCheck(order);

        for(int i = 0; i < deck.Count; i++)
        {
            if(deck[i] == null)           
                thisDeck.transform.GetChild(i).Find("SkillImage").GetComponent<Image>().color = Color.white;
            else
                thisDeck.transform.GetChild(i).Find("SkillImage").GetComponent<Image>().color = deck[i].GetComponent<Skill>().color;
        }
    }
    public void OnPointerDown(PointerEventData pointerEventData)
    {
        isClicked = true;
        StartCoroutine(touchTime());
    }

    public void OnPointerUp(PointerEventData pointerEventData)
    {
        if (clickTime >= 0.5)
        {
            Debug.Log("길게 누름");
            SkillList.SetActive(true);
        }
        else
        {
            DeckCheck(order);
        }
        StopAllCoroutines();
        Init();
    }
    public void ToggleDeck()
    {
        if (flagGroup.AnyTogglesOn())
        {
            GameManager.Instance.currentDeck = GameManager.Instance.deckList[order];
            GameManager.Instance.isSelectDeck = true;
            Debug.Log(order);
            Debug.Log("덱 삽입완료");
        }
        else
        {
            GameManager.Instance.currentDeck = null;
            GameManager.Instance.isSelectDeck = false;
            Debug.Log("덱 제거완료");
        }
    }
    public void InsertDeck(int num)
    {
        if (skillListUI.insertCheck)
        {
            for (int i = 0; i < deck.Count; i++)
            {
                if (i == num)
                    continue;
                if (deck[i] == skillListUI.selectedSkill)
                {
                    deck[i] = null;
                    thisDeck.transform.GetChild(i).Find("SkillImage").GetComponent<Image>().color = Color.white;
                }
            }
            deck[num] = skillListUI.selectedSkill;
            thisDeck.transform.GetChild(num).Find("SkillImage").GetComponent<Image>().color = skillListUI.selectedSkill.GetComponent<Skill>().color;
            
            DeckCheck(order);
            skillListUI.insertCheck = false;

            GameManager.Instance.SaveDeckInfos(order);
        }
        else
            return;
    }

    void DeckCheck(int num)
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
            //GameManager.Instance.currentDeck = GameManager.Instance.deckList[num];
            flagToggle.interactable = true;
            //Debug.Log("덱이 완성되었다.");
        }
        else
        {
            flagToggle.interactable = false;
            if (skillListUI.insertCheck)
            {
                flagGroup.SetAllTogglesOff();
                GameManager.Instance.currentDeck = null;
                Debug.Log("덱 제거완료");
            }
        }
        GameManager.Instance.isSelectDeck = flagGroup.AnyTogglesOn();
    }
    IEnumerator touchTime()
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
        }

    }
}
