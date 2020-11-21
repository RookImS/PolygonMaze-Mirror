using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class SkillUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject skill;
    
    public TextMeshProUGUI skillCostText;

    private GameObject newObject;
    private GameObject hitObject;
    private bool skillUseEnable;

    private Vector3 mousePos;

    private void Awake()
    {
        Init();
    }

    private void Update()
    {
        //UpdateSkillCostText();
    }

    private void Init()
    {
        skillCostText.text = skill.GetComponent<Skill>().cost.ToString();
    }

    private void UpdateSkillCostText()
    {
        if (skillCostText.GetComponent<Skill>().cost > PlayerControl.Instance.playerData.currentCost)
            skillUseEnable = false;
        else
            skillUseEnable = true;

        if (skillUseEnable)
            skillCostText.color = Color.white;
        else
            skillCostText.color = Color.red;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        newObject = null;
        hitObject = null;

        mousePos = Input.mousePosition;

        newObject = Instantiate(skill, mousePos, skill.transform.rotation);

        Time.timeScale = 0.3f;
    }
    public void OnDrag(PointerEventData eventData)
    {
        mousePos = Input.mousePosition;

        newObject.GetComponent<Skill>().LocateSkill(mousePos);
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        newObject.GetComponent<Skill>().UseSkill(); 

        Time.timeScale = 1f;
    }
}
