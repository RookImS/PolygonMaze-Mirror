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

    [HideInInspector] public float applyInterval;     // 스킬이 적용되는 간격
    [HideInInspector] public float applyCountDown;

    private void Awake()
    {
        transform.localScale = new Vector3(range, range, range);
        applyCountDown = 0f;
    }

    private void Update()
    {
        if (applyCountDown > 0)
            applyCountDown -= Time.deltaTime;
        else
            applyCountDown = 0;
    }

    private void Init()
    {
        transform.localScale = new Vector3(range, range, range);
        applyCountDown = 0f;
        applyInterval = 0.1f;
    }

    public void LocateSkill(Vector3 mousePos)
    {
        Vector3 realPos = Camera.main.ScreenToWorldPoint(mousePos);

        transform.position = new Vector3(realPos.x, 1.1f, realPos.z);
    }

    public void UseSkill()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            Destroy(this.gameObject);
        }
        else
        {
            GetComponent<SphereCollider>().enabled = true;
            transform.Find("Range").gameObject.SetActive(false);
            transform.Find("Effect").gameObject.SetActive(true);
            StartCoroutine("CheckSkillDuration");
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
