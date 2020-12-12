using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialChecker : MonoBehaviour
{
    [HideInInspector] public GameObject obstacles;
    [HideInInspector] public GameObject towers;
    [HideInInspector] public GameObject pauseUI;
    [HideInInspector] public GameObject deployUI;
    [HideInInspector] public GameObject skillUI;
    [HideInInspector] public GameObject waveUI;
    [HideInInspector] public CheckerBehaviour checker;
    [HideInInspector] public List<GameObject> activeList;
    [HideInInspector] public List<GameObject> restoreList;

    private float checkerDurationTemp;

    public enum UISpecific
    {
        PauseUI,
        DeployUI,
        SkillUI,
        SKillRefreshUI,
        WaveUI
    }

    protected void Start()
    {
        restoreList = new List<GameObject>();
        obstacles = GameObject.Find("Obstacles");
        towers = GameObject.Find("Towers");

        pauseUI = GameObject.Find("PauseUI");
        deployUI = GameObject.Find("DeployUI");
        skillUI = GameObject.Find("SkillUI");
        waveUI = GameObject.Find("WaveUI");
        checker = GameObject.Find("Checker").GetComponent<CheckerBehaviour>();

        checkerDurationTemp = checker.trackerDuration;
    }

    public void DisableSCExceptGameObject()
    {
        if (towers == null)
            return;
        for (int i = 0; i < towers.transform.childCount; i++)
        {
            restoreList.Add(towers.transform.GetChild(i).Find("SideCollider").gameObject);
        }

        if (obstacles == null)
            return;
        for (int i = 0; i < obstacles.transform.childCount; i++)
        {
            restoreList.Add(obstacles.transform.GetChild(i).Find("SideCollider").gameObject);
        }

        foreach (GameObject obj in activeList)
            restoreList.Remove(obj.transform.Find("SideCollider").gameObject);

        foreach (GameObject obj in restoreList)
            obj.SetActive(false);
    }

    public void DisableSCinExceptIndex(GameObject obj, int index)
    {
        Transform sc = obj.transform.Find("SideCollider");

        for (int i = 0; i < sc.childCount; i++)
        {
            restoreList.Add(sc.GetChild(i).gameObject);
        }

        restoreList.Remove(sc.GetChild(index).gameObject);
    }

    public void HighlightOneUI(UISpecific uISpecific)
    {
        if (!pauseUI || !deployUI || !skillUI || !waveUI)
            return;

        switch (uISpecific)
        {
            case UISpecific.PauseUI:
                pauseUI.transform.GetChild(0).gameObject.GetComponent<Image>().color = Color.yellow;
                break;

            case UISpecific.DeployUI:
                for (int i = 0; i < deployUI.transform.childCount; i++)
                    deployUI.transform.GetChild(i).gameObject.GetComponent<Image>().color = Color.yellow;
                break;

            case UISpecific.SkillUI:
                skillUI.transform.GetChild(0).gameObject.GetComponent<Image>().color = Color.yellow;
                break;

            case UISpecific.SKillRefreshUI:
                skillUI.transform.GetChild(1).gameObject.GetComponent<Image>().color = Color.yellow;
                break;

            case UISpecific.WaveUI:
                waveUI.transform.GetChild(0).gameObject.GetComponent<Image>().color = Color.yellow;
                break;

            default:
                break;
        }
    }

    public void HighlightDeploySlot(int index)
    {
        if (deployUI == null)
            return;

        deployUI.transform.GetChild(index).gameObject.GetComponent<Image>().color = Color.yellow;
    }

    public void UnhighlightOneUI(UISpecific uISpecific)
    {
        if (!pauseUI || !deployUI || !skillUI || !waveUI)
            return;

        switch (uISpecific)
        {
            case UISpecific.PauseUI:
                pauseUI.transform.GetChild(0).gameObject.GetComponent<Image>().color = Color.white;
                break;

            case UISpecific.DeployUI:
                for (int i = 0; i < deployUI.transform.childCount; i++)
                    deployUI.transform.GetChild(i).gameObject.GetComponent<Image>().color = Color.white;
                break;

            case UISpecific.SkillUI:
                skillUI.transform.GetChild(0).gameObject.GetComponent<Image>().color = Color.white;
                break;

            case UISpecific.SKillRefreshUI:
                skillUI.transform.GetChild(1).gameObject.GetComponent<Image>().color = Color.white;
                break;

            case UISpecific.WaveUI:
                waveUI.transform.GetChild(0).gameObject.GetComponent<Image>().color = Color.white;
                break;

            default:
                break;
        }
    }

    public void UnhighlightDeploySlot(int index)
    {
        if (deployUI == null)
            return;

        deployUI.transform.GetChild(index).gameObject.GetComponent<Image>().color = Color.white;
    }

    public void SetBaseSetting()
    {
        BlockAllUI();
        BlockChecker();
        PlayerControl.instance.SetCost(1000);
    }

    public void BlockAllUI()
    {
        if (!pauseUI || !deployUI || !skillUI || !waveUI)
            return;

        //pauseUI.transform.GetChild(0).gameObject.GetComponent<Button>().enabled = false;

        for (int i = 0; i < deployUI.transform.childCount; i++)
            deployUI.transform.GetChild(i).gameObject.GetComponent<DeployUI>().isActive = false;

        skillUI.transform.GetChild(0).gameObject.GetComponent<SkillUI>().isActive = false;
        skillUI.transform.GetChild(1).gameObject.GetComponent<Button>().enabled = false;
        waveUI.transform.GetChild(0).gameObject.GetComponent<Button>().enabled = false;
    }

    public void BlockOneUI(UISpecific uISpecific)
    {
        if (!pauseUI || !deployUI || !skillUI || !waveUI)
            return;

        switch (uISpecific)
        {
            case UISpecific.PauseUI:
                pauseUI.transform.GetChild(0).gameObject.GetComponent<Button>().enabled = false;
                break;

            case UISpecific.DeployUI:
                for (int i = 0; i < deployUI.transform.childCount; i++)
                    deployUI.transform.GetChild(i).gameObject.GetComponent<DeployUI>().isActive = false;
                break;

            case UISpecific.SkillUI:
                skillUI.transform.GetChild(0).gameObject.GetComponent<SkillUI>().isActive = false;
                break;

            case UISpecific.SKillRefreshUI:
                skillUI.transform.GetChild(1).gameObject.GetComponent<Button>().enabled = false;
                break;

            case UISpecific.WaveUI:
                waveUI.transform.GetChild(0).gameObject.GetComponent<Button>().enabled = false;
                break;

            default:
                break;
        }
    }

    public void BlockDeploySlot(int index)
    {
        if (!deployUI)
            return;

        deployUI.transform.GetChild(index).gameObject.GetComponent<DeployUI>().isActive = false;
    }

    public void RestoreBaseSetting()
    {
        RestoreAllUI();
        RestoreChecker();
        RestoreCheckerDuration();
        PlayerControl.instance.SetCost(LevelManager.instance.stageData.startCost);
    }

    public void RestoreAllUI()
    {
        if (!pauseUI || !deployUI || !skillUI || !waveUI)
            return;

        pauseUI.transform.GetChild(0).gameObject.GetComponent<Button>().enabled = true;

        for (int i = 0; i < deployUI.transform.childCount; i++)
            deployUI.transform.GetChild(i).gameObject.GetComponent<DeployUI>().isActive = true;

        skillUI.transform.GetChild(0).gameObject.GetComponent<SkillUI>().isActive = true;
        skillUI.transform.GetChild(1).gameObject.GetComponent<Button>().enabled = true;

        waveUI.transform.GetChild(0).gameObject.GetComponent<Button>().enabled = true;
    }

    public void RestoreOneUI(UISpecific uISpecific)
    {
        if (!pauseUI || !deployUI || !skillUI || !waveUI)
            return;

        switch (uISpecific)
        {
            case UISpecific.PauseUI:
                pauseUI.transform.GetChild(0).gameObject.GetComponent<Button>().enabled = true;
                break;

            case UISpecific.DeployUI:
                for (int i = 0; i < deployUI.transform.childCount; i++)
                    deployUI.transform.GetChild(i).gameObject.GetComponent<DeployUI>().isActive = true;
                break;

            case UISpecific.SkillUI:
                skillUI.transform.GetChild(0).gameObject.GetComponent<SkillUI>().isActive = true;
                break;

            case UISpecific.SKillRefreshUI:
                skillUI.transform.GetChild(1).gameObject.GetComponent<Button>().enabled = true;
                break;

            case UISpecific.WaveUI:
                waveUI.transform.GetChild(0).gameObject.GetComponent<Button>().enabled = true;
                break;

            default:
                break;
        }
    }

    public void RestoreDeploySlot(int index)
    {
        if (!deployUI)
            return;

        deployUI.transform.GetChild(index).gameObject.GetComponent<DeployUI>().isActive = true;
    }

    public void BlockChecker()
    {
        checker.isActive = false;
    }

    public void RestoreChecker()
    {
        checker.isActive = true;
    }

    public void ShowPathTracker(float duration)
    {
        checker.trackerDuration = duration;
        checker.CreateCheckerAgent();
    }

    public void RestoreCheckerDuration()
    {
        checker.trackerDuration = checkerDurationTemp;
    }

    public virtual void StartSetting(int phase)
    {

    }

    public virtual bool StartCheck(int phase)
    {
        return false;
    }

    public virtual void SettingRestore(int phase)
    {

    }

    protected void OnDestroy()
    {
        RestoreBaseSetting();
    }
}
