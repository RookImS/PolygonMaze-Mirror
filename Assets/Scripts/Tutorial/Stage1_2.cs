using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Stage1_2 : TutorialChecker
{
    private bool phase0flag;
    private GameObject phase2Obstacle1;
    private MeshCollider phase2Checker1;

    private Skill FirstSkill;
    private bool phase4Checker1;

    private void Awake()
    {
        phase0flag = false;

        phase2Obstacle1 = null;
        phase2Checker1 = null;

        FirstSkill = null;
        phase4Checker1 = false;
    }
    private new void Start()
    {
        base.Start();
        SetBaseSetting();
        PhaseM1Setter();
    }
    public override void StartSetting(int phase)
    {
        switch (phase)
        {
            case 0:
                Phase0Setter();
                break;
            case 2:
                Phase2Setter();
                break;
            case 4:
                Phase4Setter();
                break;
            case 6:
                Phase6Setter();
                break;
            default:
                return;
        }
    }

    public override bool StartCheck(int phase)
    {
        switch (phase)
        {
            case 0:
                return Phase0Checker();
            case 2:
                return Phase2Checker();
            case 4:
                return Phase4Checker();
            case 6:
                return Phase6Checker();
            default:
                return true;
        }
    }

    public override void SettingRestore(int phase)
    {
        switch (phase)
        {
            case 0:
                Phase0Complete();
                break;
            case 2:
                Phase2Complete();
                break;
            case 3:
                Phase3Complete();
                break;
            case 4:
                Phase4Complete();
                break;
            case 6:
                Phase6Complete();
                break;
            default:
                return;
        }

        foreach (GameObject obj in restoreList)
            obj.SetActive(true);

        activeList.Clear();
        restoreList.Clear();
    }

    public void PhaseM1Setter()
    {
        RestoreChecker();
        ShowPathTracker(0.5f);
    }

    public void Phase0Setter()
    {
        phase0flag = true;
    }

    public bool Phase0Checker()
    {
        return phase0flag;
    }

    public void Phase0Complete()
    {
        
    }

    public void Phase2Setter()
    {
        HighlightDeploySlot(1);
        RestoreDeploySlot(1);

        phase2Obstacle1 = obstacles.transform.GetChild(11).gameObject;

        activeList.Add(phase2Obstacle1);

        DisableSCinExceptIndex(phase2Obstacle1, 2);

        DisableSCExceptGameObject();

        phase2Checker1 = phase2Obstacle1.transform.Find("SideCollider").GetChild(2).gameObject.GetComponent<MeshCollider>();
    }

    public bool Phase2Checker()
    {
        if(phase2Checker1.enabled)
            return false;
        else
            return true;
    }

    public void Phase2Complete()
    {
        UnhighlightDeploySlot(1);
        BlockDeploySlot(1);
    }

    public void Phase3Complete()
    {
        RestoreCheckerDuration();
        BlockChecker();
    }

    public void Phase4Setter()
    {
        HighlightOneUI(UISpecific.SkillUI);
        RestoreOneUI(UISpecific.SkillUI);
    }

    public bool Phase4Checker()
    {
        if (skillUI.transform.GetChild(0).GetComponent<SkillUI>().newObject != null)
        {
            FirstSkill = skillUI.transform.GetChild(0).GetComponent<SkillUI>().newObject.GetComponent<Skill>();
            phase4Checker1 = FirstSkill.effectObject.activeSelf;
        }

        return phase4Checker1;
    }

    public void Phase4Complete()
    {
        UnhighlightOneUI(UISpecific.SkillUI);
        BlockOneUI(UISpecific.SkillUI);
    }

    public void Phase6Setter()
    {
        HighlightOneUI(UISpecific.SKillRefreshUI);
        RestoreOneUI(UISpecific.SKillRefreshUI);
    }

    public bool Phase6Checker()
    {
        return skillUI.transform.GetChild(0).GetComponent<SkillUI>().isFirst;
    }

    public void Phase6Complete()
    {
        UnhighlightOneUI(UISpecific.SKillRefreshUI);
        BlockOneUI(UISpecific.SKillRefreshUI);
    }
}