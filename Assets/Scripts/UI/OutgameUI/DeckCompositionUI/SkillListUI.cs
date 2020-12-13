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
    private string rangeText;

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
        skillInfo.skillName.text = null;
        skillInfo.skillDesc.text = null;
        skillInfo.skillCost.text = null;
        skillInfo.skillDuration.text = null;
        skillInfo.skillRange.text = null;
        for (int i = 0; i < GameManager.Instance.skills.Count; i++)
        {
            GameObject skill = GameManager.Instance.skills[i];
            GameObject skillSlot = skillSlots.transform.GetChild(i).gameObject;

            skillSlot.GetComponent<Button>().onClick.AddListener(() => SelectSkill(skill));
            skillSlot.transform.GetChild(0).GetComponent<Image>().color = skill.GetComponent<Skill>().color;
        }
    }

    public void SelectSkill(GameObject skill)
    {
        selectedSkill = skill;

        skillPanel.transform.Find("SkillImage").gameObject.GetComponent<Image>().color = selectedSkill.GetComponent<Skill>().color;
        skillInfo.skillName.text = selectedSkill.GetComponent<Skill>().id;
        skillInfo.skillDesc.text = selectedSkill.GetComponent<Skill>().desc;
        skillInfo.skillCost.text = selectedSkill.GetComponent<Skill>().cost.ToString();
        skillInfo.skillDuration.text = selectedSkill.GetComponent<Skill>().skillDuration.ToString() + " 초";
        if(selectedSkill.GetComponent<Skill>().range > 0)
        {
            rangeText = "매우좁음";
            if (selectedSkill.GetComponent<Skill>().range >= 1)
            {
                rangeText = "좁음";
                if (selectedSkill.GetComponent<Skill>().range >= 2)
                {
                    rangeText = "보통";
                    if (selectedSkill.GetComponent<Skill>().range >= 3)
                    {
                        rangeText = "넓음";
                    }
                }
            }
        }
        skillInfo.skillRange.text = rangeText + "(" + selectedSkill.GetComponent<Skill>().range.ToString() +")";

        if (SoundManager.Instance != null)
            SoundManager.instance.PlaySound(SoundManager.SoundSpecific.BUTTON, "Common_Button");

    }

    public void GetInsertSkill()
    {
        insertCheck = true;

        if (SoundManager.Instance != null)
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

        if (SoundManager.instance != null)
            SoundManager.instance.PlaySound(SoundManager.SoundSpecific.BUTTON, "Cancle_Button");
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
