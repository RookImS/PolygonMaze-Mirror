using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DeckUI : MonoBehaviour
{
    private Skill SelectedSkill;
    private Skill insertSkill;

    public SkillToopTip ToopTip;
    public SelectedPanel SelectedPanel;
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
}
