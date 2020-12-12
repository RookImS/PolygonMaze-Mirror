using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Stage1_1 : TutorialChecker
{
    private GameObject phase6Obstacle1, phase6Obstacle2;
    private MeshCollider phase6Checker1, phase6Checker2;

    private GameObject phase8Tower1;
    private MeshCollider phase8Checker1;

    private GameObject phase10Obstacle1;
    private DeployUI phase10DeployUI1;

    private void Awake()
    {
        phase6Obstacle1 = phase6Obstacle2 = null;
        phase6Checker1 = phase6Checker2 = null;
        phase8Tower1 = null;
        phase8Checker1 = null;
        phase10Obstacle1 = null;
        phase10DeployUI1 = null;
    }

    private new void Start()
    {
        base.Start();
        SetBaseSetting();
    }

    public override void StartSetting(int phase)
    {
        switch (phase)
        {
            case 6:
                Phase6Setter();
                break;
            case 8:
                Phase8Setter();
                break;
            case 10:
                Phase10Setter();
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
            case 8:
                return Phase8Checker();
            case 10:
                return Phase10Checker();
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
            case 8:
                Phase8Complete();
                break;
            case 10:
                Phase10Complete();
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
        HighlightDeploySlot(1);
        RestoreDeploySlot(1);

        phase6Obstacle1 = obstacles.transform.GetChild(4).gameObject;
        phase6Obstacle2 = obstacles.transform.GetChild(5).gameObject;

        activeList.Add(phase6Obstacle1);
        activeList.Add(phase6Obstacle2);

        DisableSCinExceptIndex(phase6Obstacle1, 2);
        DisableSCinExceptIndex(phase6Obstacle2, 2);

        DisableSCExceptGameObject();

        phase6Checker1 = phase6Obstacle1.transform.Find("SideCollider").GetChild(2).gameObject.GetComponent<MeshCollider>();
        phase6Checker2 = phase6Obstacle2.transform.Find("SideCollider").GetChild(2).gameObject.GetComponent<MeshCollider>();

    }

    private bool Phase6Checker()
    {
        if (phase6Checker1.enabled || phase6Checker2.enabled)
            return false;
        else
            return true;
    }

    private void Phase6Complete()
    {
        UnhighlightDeploySlot(1);
        BlockDeploySlot(1);
    }

    private void Phase8Setter()
    {
        HighlightDeploySlot(2);
        RestoreDeploySlot(2);

        float x, z;

        for(int i = 0; i < towers.transform.childCount; i++)
        {
            x = towers.transform.GetChild(i).position.x;
            z = towers.transform.GetChild(i).position.z;

            if ((-0.005f < x && x < 0.005f)
                && (1.09f < z && z < 1.10f))
                phase8Tower1 = towers.transform.GetChild(i).gameObject;
        }

        activeList.Add(phase8Tower1);

        DisableSCinExceptIndex(phase8Tower1, 2);

        DisableSCExceptGameObject();

        phase8Checker1 = phase8Tower1.transform.Find("SideCollider").GetChild(2).gameObject.GetComponent<MeshCollider>();
    }

    private bool Phase8Checker()
    {
        if (phase8Checker1.enabled)
            return false;
        else
            return true;
    }

    private void Phase8Complete()
    {
        UnhighlightDeploySlot(2);
        BlockDeploySlot(2);
    }

    private void Phase10Setter()
    {
        HighlightDeploySlot(3);
        RestoreDeploySlot(3);

        phase10Obstacle1 = obstacles.transform.GetChild(12).gameObject;
        phase10DeployUI1 = deployUI.transform.GetChild(3).GetComponent<DeployUI>();

        activeList.Add(phase10Obstacle1);

        DisableSCinExceptIndex(phase10Obstacle1, 2);

        DisableSCExceptGameObject();
    }

    private bool Phase10Checker()
    {
        
        if (phase10Obstacle1.Equals(phase10DeployUI1.hitObject) && !phase10DeployUI1.isProgressDeploy)
            return true;
        else
            return false;
    }

    private void Phase10Complete()
    {
        UnhighlightDeploySlot(3);
        BlockDeploySlot(3);
    }
}
