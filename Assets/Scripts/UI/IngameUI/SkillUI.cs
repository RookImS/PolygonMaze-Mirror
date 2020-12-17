using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class SkillUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [HideInInspector] public GameObject skill;
    public int skillRefreshCost;

    public Image skillImage;
    public CanvasGroup slotCanvasGroup;
    public CanvasGroup refreshCanvasGroup;
    public TextMeshProUGUI skillCostText;
    public TextMeshProUGUI skillRefreshCostText;
    [HideInInspector] public bool isActive;
    [HideInInspector] public bool isFirst;

    [HideInInspector] public GameObject newObject;
    private bool skillUseEnable;
    private bool skillRefreshEnable;

    private Vector3 mousePos;

    private void Awake()
    {
        Init();
    }

    private void Update()
    {
        UpdateSkillCostText();
        if (!isActive || GameManager.Instance.isGameOver || GameManager.Instance.isStageClear)
        {
            if(newObject != null)
                Destroy(newObject);
        }
    }

    private void Init()
    {
        SelectNextSkill();
        isActive = true;
        isFirst = false;
        skillRefreshCostText.text = skillRefreshCost.ToString();
    }

    private void UpdateSkillCostText()
    {
        if (PlayerControl.Instance.CheckCost(skill.GetComponent<Skill>().cost))
            skillUseEnable = true;
        else
            skillUseEnable = false;

        if (skillUseEnable)
        {
            skillCostText.color = Color.white;
            slotCanvasGroup.blocksRaycasts = true;
        }
        else
        {
            skillCostText.color = Color.red;
            slotCanvasGroup.blocksRaycasts = false;
        }

        if (PlayerControl.Instance.CheckCost(skillRefreshCost))
            skillRefreshEnable = true;
        else
            skillRefreshEnable = false;

        if (skillRefreshEnable)
        {
            skillRefreshCostText.color = Color.white;
            refreshCanvasGroup.blocksRaycasts = true;
        }
        else
        {
            skillRefreshCostText.color = Color.red;
            refreshCanvasGroup.blocksRaycasts = false;
        }
    }

    public void SelectNextSkill()
    {
        int randomValue = Random.Range(0, GameManager.Instance.currentDeck.Count);
        skill = GameManager.Instance.currentDeck[randomValue];

        skillCostText.text = skill.GetComponent<Skill>().cost.ToString();
        skillImage.color = skill.GetComponent<Skill>().color;
    }

    public void SkillRefresh()
    {
        if (PlayerControl.Instance.UseCost(skillRefreshCost))
        {
            SelectNextSkill();
            isFirst = true;
        }
    }

    public void OnMouseDown()
    {
        if (isActive)
        {
            if(SoundManager.Instance != null)
                SoundManager.Instance.PlaySound(SoundManager.SoundSpecific.BUTTON, "Tower_Button");
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isActive)
        {
            newObject = null;
            UIManager.instance.infoUI.DisableInfo();
            UIManager.instance.BlockRaycastOff();

            mousePos = Input.mousePosition;

            newObject = Instantiate(skill, mousePos, skill.transform.rotation);

            if (Tutorial.instance.tutorialChecker == null && Time.timeScale != 0f)
                GameManager.instance.SlowTime(0.3f);
        }
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (isActive && !GameManager.Instance.isGameOver && !GameManager.Instance.isStageClear)
        {
            mousePos = Input.mousePosition;

            newObject.GetComponent<Skill>().LocateSkill(mousePos);
        }
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (isActive && !GameManager.Instance.isGameOver && !GameManager.Instance.isStageClear)
        {
            mousePos = Input.mousePosition;

            if (newObject.GetComponent<Skill>().UseSkill(mousePos))
                SelectNextSkill();

            UIManager.instance.BlockRaycastOn();

            if (Tutorial.instance.tutorialChecker == null && Time.timeScale != 0f)
                GameManager.instance.TimeRestore();
        }
    }
}
