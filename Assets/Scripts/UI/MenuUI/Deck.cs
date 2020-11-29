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
        SelectedPanel.Panelimage.color = SelectedSkill.GetComponent<Skill>().color; ;
        SelectedPanel.gameObject.SetActive(true);
        //SelectedPanel.transform.SetParent(EventSystem.current.currentSelectedGameObject.transform, false);
        SelectedPanel.transform.position = new Vector2(EventSystem.current.currentSelectedGameObject.transform.position.x, EventSystem.current.currentSelectedGameObject.transform.position.y -33);
    }
    public void skillInfo()
    {
        ToopTip.gameObject.SetActive(true);
        ToopTip.Name.text = SelectedSkill.GetComponent<Skill>().id;
        ToopTip.Desc.text = SelectedSkill.GetComponent<Skill>().desc;
        ToopTip.Skillimage.color = SelectedSkill.GetComponent<Skill>().color;
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
            for (int i = 0; i < GameManager.Instance.deck1.Count; i++)
            {
                if (i == num)
                    continue;
                if (GameManager.Instance.deck1[i] == insertSkill)
                {
                    GameManager.Instance.deck1[i] = null;
                    this.transform.GetChild(i).Find("Image").GetComponent<Image>().color = Color.white;
                }
            }
            GameManager.Instance.deck1[num] = insertSkill;
            this.transform.GetChild(num).Find("Image").GetComponent<Image>().color = insertSkill.GetComponent<Skill>().color;
            insertcheck = false;
        }
        else
            return;
    }
}
