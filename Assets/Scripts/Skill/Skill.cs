using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Skill : MonoBehaviour
{
    public float skillDuration;     // 스킬 자체의 유지시간
    public float applyDuration;     // 걸린 스킬의 유지시간

    public string id;
    public string desc;
    public float range;
    public int cost;
    public Sprite skillSprite;
    public Color color;

    //[HideInInspector] public float applyInterval;     // 스킬이 적용되는 간격
    //[HideInInspector] public float applyCountDown;

    private bool isEnoughMoney;
    private bool isDeploy;

    private void Awake()
    {
        Init();
    }

    private void Update()
    {
        //if (!isDeploy)
        //{
        //    isEnoughMoney = PlayerControl.Instance.CheckCost(cost);
        //}
    }

    private void Init()
    {
        transform.localScale = new Vector3(range, range, range);
        //applyCountDown = 0f;
        //applyInterval = 0.1f;

        //isEnoughMoney = false;
        isDeploy = false;
    }

    public void LocateSkill(Vector3 mousePos)
    {
        Vector3 realPos = Camera.main.ScreenToWorldPoint(mousePos);

        transform.position = new Vector3(realPos.x, 1.1f, realPos.z);
    }

    public bool UseSkill()
    {
        if (EventSystem.current.IsPointerOverGameObject()/* || !isEnoughMoney*/)
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
            isDeploy = true;

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
