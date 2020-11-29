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
        SelectNextSkill();
        //skillCostText.text = skill.GetComponent<Skill>().cost.ToString();
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

    private void SelectNextSkill()
    {
        int randomValue = Random.Range(0, GameManager.Instance.currentDeck.Count);
        skill = GameManager.Instance.currentDeck[randomValue];
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        newObject = null;
        hitObject = null;
        UIManager.instance.infoUI.DisableInfo();
        UIManager.instance.canvasGroup.blocksRaycasts = false;

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
        if (newObject.GetComponent<Skill>().UseSkill())
            SelectNextSkill();

        UIManager.instance.canvasGroup.blocksRaycasts = true;
        Time.timeScale = 1f;
    }
}
