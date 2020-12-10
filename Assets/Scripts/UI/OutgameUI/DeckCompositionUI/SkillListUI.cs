using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class SkillListUI : MonoBehaviour
{
    public GameObject selectedSkill;
    public GameObject skillSlots;
    public GameObject skillPanel;
    public SkillInfo skillInfo;
    public CanvasGroup Deck0;
    public CanvasGroup Deck1;
    public CanvasGroup Deck2;
    public ToggleGroup flagGroup;
    public GameObject bgPanel;
    public bool insertCheck;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        InitSkillSlot();
        insertCheck = false;
    }

    void InitSkillSlot()
    {
        skillInfo.Name.text = null;
        skillInfo.Desc.text = null;
        for (int i = 0; i < GameManager.Instance.skills.Count; i++)
        {
            GameObject skill = GameManager.Instance.skills[i];
            GameObject skillSlot = skillSlots.transform.GetChild(i).gameObject;

            skillSlot.GetComponent<Button>().onClick.AddListener(() => SelectSkill(skill));
            skillSlot.transform.GetChild(0).GetComponent<Image>().color = skill.GetComponent<Skill>().color;
        }
    }

    //void InitSkillInfo()
    //{
    //    //skillInfo.gameObject.SetActive(false);
    //    skillInfo.Name.text = null;
    //    skillInfo.Desc.text = null;
    //    //skillInfo.Skillimage.color = Color.white;
    //}

    public void SelectSkill(GameObject skill)
    {
        selectedSkill = skill;
        skillPanel.transform.Find("SkillImage").gameObject.GetComponent<Image>().color = selectedSkill.GetComponent<Skill>().color;
        skillInfo.Name.text = selectedSkill.GetComponent<Skill>().id;
        skillInfo.Desc.text = selectedSkill.GetComponent<Skill>().desc;
        skillPanel.gameObject.SetActive(true);

        GameObject selectedSlot = EventSystem.current.currentSelectedGameObject;

        skillPanel.transform.position = selectedSlot.transform.position;

        Vector2 recPos = skillPanel.transform.GetComponent<RectTransform>().anchoredPosition;
        float correctPos_x =
            (skillPanel.GetComponent<RectTransform>().rect.width - selectedSlot.GetComponent<RectTransform>().rect.width) / 2;
        float correctPos_y = 
            skillPanel.GetComponent<RectTransform>().rect.height / 2;
        skillPanel.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(recPos.x + correctPos_x, recPos.y - correctPos_y);
    }

    //public void ActiveSkillInfo()
    //{
    //    skillInfo.Name.text = selectedSkill.GetComponent<Skill>().id;
    //    skillInfo.Desc.text = selectedSkill.GetComponent<Skill>().desc;
    //    SoundManager.Instance.PlaySound(SoundManager.SoundSpecific.BUTTON, "Common_Button");
    //}

    public void GetInsertSkill()
    {
        insertCheck = true;

        SoundManager.Instance.PlaySound(SoundManager.SoundSpecific.BUTTON, "Common_Button");
    }

    void OnEnable()
    {
        //Deck0.interactable = true;
        //Deck1.interactable = true;
        //Deck2.interactable = true;
        Deck0.blocksRaycasts = true;
        Deck1.blocksRaycasts = true;
        Deck2.blocksRaycasts = true;
        //flagGroup.SetAllTogglesOff();
        bgPanel.SetActive(true);
    }
    void OnDisable()
    {
        //Deck0.interactable = false;
        //Deck1.interactable = false;
        //Deck2.interactable = false;
        Deck0.blocksRaycasts = false;
        Deck1.blocksRaycasts = false;
        Deck2.blocksRaycasts = false;
        //flagGroup.SetAllTogglesOff();
        bgPanel.SetActive(false);
    }
}
