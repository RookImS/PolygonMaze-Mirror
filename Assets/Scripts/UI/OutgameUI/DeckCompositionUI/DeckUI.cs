using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DeckUI : MonoBehaviour
{
    public SkillListUI skillListUI;
    public int order;

    private List<GameObject> deck;

    private void Awake()
    {
        Init();
    }
    private void Init()
    {
        deck = GameManager.Instance.deckList[order];
        
        for(int i = 0; i < deck.Count; i++)
        {
            if(deck[i] == null)           
                transform.GetChild(i).Find("SkillImage").GetComponent<Image>().color = Color.white;
            else
                transform.GetChild(i).Find("SkillImage").GetComponent<Image>().color = deck[i].GetComponent<Skill>().color;
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
                    this.transform.GetChild(i).Find("SkillImage").GetComponent<Image>().color = Color.white;
                }
            }
            deck[num] = skillListUI.selectedSkill;
            this.transform.GetChild(num).Find("SkillImage").GetComponent<Image>().color = skillListUI.selectedSkill.GetComponent<Skill>().color;
            
            skillListUI.insertCheck = false;
            GameManager.Instance.SaveDeckData();
        }
        else
            return;
    }
}
