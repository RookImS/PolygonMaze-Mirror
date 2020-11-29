using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Deck : MonoBehaviour
{
    public static Deck instance;

    private GameObject SelectedSkill;
    private GameObject insertSkill;
    private bool insertcheck = false;

    public SkillToopTip ToopTip;
    public SelectedPanel SelectedPanel;

    private void Awake()
    {
        instance = this;
        skillAddInit();
    }
    public void viewPanel(GameObject skill)
    {
        SelectedSkill = skill;
        SelectedPanel.Panelimage.color = SelectedSkill.GetComponent<Skill>().skillColor; ;
        SelectedPanel.gameObject.SetActive(true);
        //SelectedPanel.transform.SetParent(EventSystem.current.currentSelectedGameObject.transform, false);
        SelectedPanel.transform.position = new Vector2(EventSystem.current.currentSelectedGameObject.transform.position.x, EventSystem.current.currentSelectedGameObject.transform.position.y -33);
    }
    public void skillInfo()
    {
        ToopTip.gameObject.SetActive(true);
        ToopTip.Name.text = SelectedSkill.GetComponent<Skill>().itemName;
        ToopTip.Desc.text = SelectedSkill.GetComponent<Skill>().itemDesc;
        ToopTip.Skillimage.color = SelectedSkill.GetComponent<Skill>().skillColor;
    }
    void skillAddInit()
    {
        ToopTip.gameObject.SetActive(false);
        ToopTip.Name.text = null;
        ToopTip.Desc.text = null;
        ToopTip.Skillimage.color = Color.white;
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
                    this.transform.GetChild(i).Find("Image").GetComponent<Image>().color = Color.white;
                }
            }
            GameManager.Deck[num] = insertSkill;
            this.transform.GetChild(num).Find("Image").GetComponent<Image>().color = insertSkill.GetComponent<Skill>().skillColor;
            insertcheck = false;
        }
        else
            return;
    }
}
