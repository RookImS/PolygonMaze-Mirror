using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class Skill : MonoBehaviour
{
    public float skillDuration;     // 스킬 자체의 유지시간
    public float applyDuration;     // 걸린 스킬의 유지시간

    public Color color;
    public float range;
    public int cost;
    public string id;
    [TextArea]
    public string desc;

    public GameObject rangeObject;
    public GameObject effectObject;

    private AudioSource currentAudioSource1;
    public SoundManager.SkillSoundSpecific skillSoundSpecific;
    public string ready_sound_name;
    public string use_sound_name;
    public string effect_sound_name;
    public string apply_sound_name;

    public GameObject VFX;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        transform.localScale = new Vector3(range, range, range);
        if (SoundManager.instance != null)
        {
            currentAudioSource1 = SoundManager.Instance.GetLoopSkillAudio();
            SoundManager.Instance.PlayLoopSkillSound(currentAudioSource1, skillSoundSpecific, ready_sound_name);
        }
    }

    public void LocateSkill(Vector3 mousePos)
    {
        Vector3 realPos = Camera.main.ScreenToWorldPoint(mousePos);

        transform.position = new Vector3(realPos.x, 1.1f, realPos.z);
    }

    public bool UseSkill(Vector3 mousePos)
    {
        bool isCheckUI;

        if (Application.platform == RuntimePlatform.Android)
            isCheckUI = EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
        else
            isCheckUI = EventSystem.current.IsPointerOverGameObject();

        if (!isCheckUI)
        {
            if (SoundManager.instance != null)
                SoundManager.instance.StopAudio(currentAudioSource1);

            Destroy(this.gameObject);

            return false;
        }
        else
        {
            PlayerControl.Instance.UseCost(cost);
            GetComponent<SphereCollider>().enabled = true;
            rangeObject.SetActive(false);
            effectObject.SetActive(true);
            StartCoroutine("CheckSkillDuration");

            if (SoundManager.instance != null)
            {
                SoundManager.Instance.PlaySkillSound(skillSoundSpecific, use_sound_name);

                if (effect_sound_name != "Space")
                    SoundManager.Instance.PlayLoopSkillSound(currentAudioSource1, skillSoundSpecific, effect_sound_name);
            }

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

    public void OnDestroy()
    {
        if (SoundManager.instance != null)
            SoundManager.Instance.StopAudio(currentAudioSource1);
    }
}
