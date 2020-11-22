using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Deck : MonoBehaviour
{
    public static Deck instance;

    private Skill SelectedSkill;
    private Skill insertSkill;
    private bool insertcheck = false;

    public SkillToopTip ToopTip;
    public SelectedPanel SelectedPanel;

    private void Awake()
    {
        instance = this;
        skillAddInit();
    }
    public void viewPanel(Skill skill)
    {
        SelectedSkill = skill;
        SelectedPanel.Panelimage.sprite = SelectedSkill.itemImage;
        SelectedPanel.gameObject.SetActive(true);
        SelectedPanel.transform.SetParent(EventSystem.current.currentSelectedGameObject.transform, false);
    }

    public void skillInfo()
    {
        ToopTip.gameObject.SetActive(true);
        ToopTip.Name.text = SelectedSkill.itemName;
        ToopTip.Desc.text = SelectedSkill.itemDesc;
        ToopTip.Skillimage.sprite = SelectedSkill.itemImage;
    }

    void skillAddInit()
    {
        ToopTip.gameObject.SetActive(false);
        ToopTip.Name.text = null;
        ToopTip.Desc.text = null;
        ToopTip.Skillimage.sprite = null;
    }

    public void Insert()
    {
        insertSkill = SelectedSkill;
        insertcheck = true;
    }
    public void InsertDeck(int num)
    {
        if (insertcheck)
        {
            for (int i = 0; i < GameManager.Deck.Count; i++)
            {
                if (i == num)
                    continue;
                if (GameManager.Deck[i] == insertSkill)
                {
                    GameManager.Deck[i] = null;
                    this.transform.GetChild(i).Find("Image").GetComponent<Image>().sprite = null;
                }
            }
            GameManager.Deck[num] = insertSkill;
            this.transform.GetChild(num).Find("Image").GetComponent<Image>().sprite = insertSkill.itemImage;

            insertcheck = false;
        }
        else
            return;
    }
}
