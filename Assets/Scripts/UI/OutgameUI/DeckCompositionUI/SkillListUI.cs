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
    public bool insertCheck;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        InitSkillSlot();
        InitSkillInfo();

        insertCheck = false;
    }

    void InitSkillSlot()
    {
        for (int i = 0; i < GameManager.Instance.skills.Count; i++)
        {
            GameObject skill = GameManager.Instance.skills[i];
            GameObject skillSlot = skillSlots.transform.GetChild(i).gameObject;
            skillSlot.GetComponent<Button>().onClick.AddListener(() => SelectSkill(skill));
            skillSlot.transform.GetChild(0).GetComponent<Image>().color = skill.GetComponent<Skill>().color;
        }
    }
    void InitSkillInfo()
    {
        skillInfo.gameObject.SetActive(false);
        skillInfo.Name.text = null;
        skillInfo.Desc.text = null;
        skillInfo.Skillimage.color = Color.white;
    }

    public void SelectSkill(GameObject skill)
    {
        selectedSkill = skill;
        skillPanel.transform.Find("SkillImage").gameObject.GetComponent<Image>().color = selectedSkill.GetComponent<Skill>().color; ;
        skillPanel.gameObject.SetActive(true);

        GameObject selectedSlot = EventSystem.current.currentSelectedGameObject;

        skillPanel.transform.position = new Vector2(selectedSlot.transform.position.x, selectedSlot.transform.position.y);

        Vector2 recPos = skillPanel.transform.GetComponent<RectTransform>().anchoredPosition;
        float correctPos = 
            (skillPanel.GetComponent<RectTransform>().rect.height - selectedSlot.GetComponent<RectTransform>().rect.height) / 2;
        skillPanel.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(recPos.x, recPos.y - correctPos);
    }

    public void ActiveSkillInfo()
    {
        skillInfo.gameObject.SetActive(true);
        skillInfo.Name.text = selectedSkill.GetComponent<Skill>().id;
        skillInfo.Desc.text = selectedSkill.GetComponent<Skill>().desc;
        skillInfo.Skillimage.color = selectedSkill.GetComponent<Skill>().color;
    }

    public void GetInsertSkill()
    {
        insertCheck = true;
    }
}
