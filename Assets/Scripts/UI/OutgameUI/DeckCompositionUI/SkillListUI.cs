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
    public List<CanvasGroup> deckList;
    public List<CanvasGroup> deckPanelList;
    public ToggleGroup flagGroup;
    public GameObject bgPanel;
    public bool insertCheck;

    private int selectedDeck;

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

    public void HighlightDeck(int num)
    {
        selectedDeck = num;

        for (int i = 0; i < deckPanelList.Count; i++)
        {
            if (i == num)
                continue;

            deckPanelList[i].blocksRaycasts = false;
            deckPanelList[i].interactable = false;
            deckPanelList[i].alpha = 0.3f;
        }
    }

    public void ExitSkillList()
    {
        for (int i = 0; i < deckPanelList.Count; i++)
        {
            if (i == selectedDeck)
                continue;

            deckPanelList[i].blocksRaycasts = true;
            deckPanelList[i].interactable = true;
            deckPanelList[i].alpha = 1f;
        }

        deckPanelList[selectedDeck].gameObject.GetComponent<DeckUI>().DeckCheck(selectedDeck);
        gameObject.SetActive(false);
    }

    void OnEnable()
    {
        foreach (CanvasGroup deck in deckList)
        {
            deck.blocksRaycasts = true;
        }
        bgPanel.SetActive(true);
    }
    void OnDisable()
    {
        foreach (CanvasGroup deck in deckList)
        {
            deck.blocksRaycasts = false;
        }
        bgPanel.SetActive(false);
    }
}
