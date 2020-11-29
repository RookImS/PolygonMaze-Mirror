using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Skill : MonoBehaviour
{
    public float skillDuration;     // 스킬 자체의 유지시간
    public float applyDuration;     // 걸린 스킬의 유지시간

    public string id;
    public float range;
    public int cost;
    public Sprite skillSprite;
    public Renderer rend;

    [HideInInspector] public float applyInterval;     // 스킬이 적용되는 간격
    [HideInInspector] public float applyCountDown;

    private bool isEnoughMoney;

    private void Awake()
    {
        Init();
    }

    private void Update()
    {
        if (applyCountDown > 0)
            applyCountDown -= Time.deltaTime;
        else
            applyCountDown = 0;

        isEnoughMoney = PlayerControl.Instance.CheckCost(cost);
        
        if(isEnoughMoney)
        {
            rend.material.color = new Color(1f, 1f, 1f, 170 / 255f);
        }
        else
        {
            rend.material.color = new Color(1f, 170 / 255f, 170 / 255f, 170 / 255f);
        }
    }

    private void Init()
    {
        transform.localScale = new Vector3(range, range, range);
        applyCountDown = 0f;
        applyInterval = 0.1f;

        isEnoughMoney = false;
    }

    public void LocateSkill(Vector3 mousePos)
    {
        Vector3 realPos = Camera.main.ScreenToWorldPoint(mousePos);

        transform.position = new Vector3(realPos.x, 1.1f, realPos.z);
    }

    public bool UseSkill()
    {
        if (EventSystem.current.IsPointerOverGameObject() || !isEnoughMoney)
        {
            Destroy(this.gameObject);

            return false;
        }
        else
        {
            PlayerControl.Instance.UseCost(cost);
            GetComponent<SphereCollider>().enabled = true;
            transform.Find("Range").gameObject.SetActive(false);
            transform.Find("Effect").gameObject.SetActive(true);
            StartCoroutine("CheckSkillDuration");

            return true;
        }

    }

    public virtual void ApplySkill(GameObject obj)
    {

    }

    public IEnumerator CheckSkillDuration()
    {
        yield return new WaitForSeconds(skillDuration);

        Destroy(this.gameObject);
    }
}
