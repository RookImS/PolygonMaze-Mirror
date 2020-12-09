using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Stage1 : TutorialChecker
{
    private GameObject phase6Tower;
    private MeshCollider phase6Checker;
    private GameObject phase9Tower1, phase9Tower2;
    private MeshCollider phase9Checker1, phase9Checker2;
    private GameObject phase11Tower;
    private DeployUI phase11DeployUI;

    public override void StartSetting(int phase)
    {
        switch (phase)
        {
            case 6:
                Phase6Setter();
                break;
            case 9:
                Phase9Setter();
                break;
            case 11:
                Phase11Setter();
                break;
            default:
                break;
        }
    }
    public override bool StartCheck(int phase)
    {
        switch (phase)
        {
            case 6:
                return Phase6Checker();
            case 9:
                return Phase9Checker();
            case 11:
                return Phase11Checker();
            default:
                return true;
        }
    }
    public override void SettingRestore(int phase)
    {
        switch (phase)
        {
            case 6:
                Phase6Complete();
                break;
            case 9:
                Phase9Complete();
                break;
            case 11:
                Phase11Complete();
                break;
            default:
                break;
        }

        foreach (GameObject obj in restoreList)
            obj.SetActive(true);

        activeList.Clear();
        restoreList.Clear();
    }
    private void Phase6Setter()
    {
        HighlightDeploySlot(0);

        const int towerNo = 4;

        phase6Tower = obstacles.transform.GetChild(towerNo).gameObject;
        activeList.Add(phase6Tower);

        DisableSCinExceptIndex(phase6Tower, 2);

        DisableSCExceptGameObject();

        phase6Checker = phase6Tower.transform.Find("SideCollider").GetChild(2).gameObject.GetComponent<MeshCollider>();
    }
    private bool Phase6Checker()
    {
        if (phase6Checker.enabled)
            return false;
        else
            return true;
    }
    private void Phase6Complete()
    {
        RestoreDeploySlot(0);
    }

    private void Phase9Setter()
    {
        HighlightDeploySlot(1);

        const int towerNo1 = 0;
        const int towerNo2 = 11;

        phase9Tower1 = obstacles.transform.GetChild(towerNo1).gameObject;
        phase9Tower2 = obstacles.transform.GetChild(towerNo2).gameObject;

        activeList.Add(phase9Tower1);
        activeList.Add(phase9Tower2);

        DisableSCinExceptIndex(phase9Tower1, 2);
        DisableSCinExceptIndex(phase9Tower2, 2);

        DisableSCExceptGameObject();

        phase9Checker1 = phase9Tower1.transform.Find("SideCollider").GetChild(2).gameObject.GetComponent<MeshCollider>();
        phase9Checker2 = phase9Tower2.transform.Find("SideCollider").GetChild(2).gameObject.GetComponent<MeshCollider>();

    }

    private bool Phase9Checker()
    {
        if (phase9Checker1.enabled || phase9Checker2.enabled)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    private void Phase9Complete()
    {
        RestoreDeploySlot(1);
    }
    private void Phase11Setter()
    {
        HighlightDeploySlot(3);

        const int towerNo = 10;

        phase11Tower = obstacles.transform.GetChild(towerNo).gameObject;
        phase11DeployUI = deployUI.transform.GetChild(3).GetComponent<DeployUI>();
        activeList.Add(phase11Tower);

        DisableSCinExceptIndex(phase11Tower, 3);

        DisableSCExceptGameObject();
    }
    private bool Phase11Checker()
    {
        if (phase11Tower.Equals(phase11DeployUI.hitObject) && !phase11DeployUI.isProgressDeploy)
            return true;
        else
            return false;
    }
    private void Phase11Complete()
    {
        RestoreDeploySlot(3);
    }
}
