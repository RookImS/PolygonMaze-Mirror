using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class SkillUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [HideInInspector] public GameObject skill;
    public Image skillImage;
    public CanvasGroup canvasGroup;
    public TextMeshProUGUI skillCostText;

    private GameObject newObject;
    private bool skillUseEnable;

    private Vector3 mousePos;

    private void Awake()
    {
        Init();
    }

    private void Update()
    {
        UpdateSkillCostText();
    }

    private void Init()
    {
        SelectNextSkill();
    }

    private void UpdateSkillCostText()
    {
        if (skill.GetComponent<Skill>().cost > PlayerControl.Instance.playerData.currentCost)
            skillUseEnable = false;
        else
            skillUseEnable = true;

        if (skillUseEnable)
        {
            skillCostText.color = Color.white;  
            canvasGroup.blocksRaycasts = true;
        }
        else
        {
            skillCostText.color = Color.red;
            canvasGroup.blocksRaycasts = false;
        }
    }

    private void SelectNextSkill()
    {
        int randomValue = Random.Range(0, GameManager.Instance.currentDeck.Count);
        skill = GameManager.Instance.currentDeck[randomValue];

        skillCostText.text = skill.GetComponent<Skill>().cost.ToString();
        skillImage.color = skill.GetComponent<Skill>().color;
    }

    public void OnMouseDown()
    {
        SoundManager.Instance.PlaySound(SoundManager.SoundSpecific.BUTTON, "Tower_Button");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
            newObject = null;
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
