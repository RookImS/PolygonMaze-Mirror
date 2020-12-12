using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Stage1_4 : TutorialChecker
{
    private bool phase2CheckerFlag1;
    private bool phase2CheckerFlag2;

    private bool phase3CheckerFlag1;

    private bool phase4CheckerFlag1;
    private bool phase4CheckerFlag2;

    private bool phase5CheckerFlag1;

    private void Awake()
    {
        phase2CheckerFlag1 = false;
        phase2CheckerFlag2 = false;
        phase3CheckerFlag1 = false;
        phase4CheckerFlag1 = false;
        phase4CheckerFlag2 = false;
        phase5CheckerFlag1 = false;
    }

    private new void Start()
    {
        base.Start();
        BlockChecker();
    }

    public override void StartSetting(int phase)
    {
        switch (phase)
        {
            default:
                return;
        }
    }

    public override bool StartCheck(int phase)
    {
        switch (phase)
        {
            case 2:
                return Phase2Checker();
            case 3:
                return Phase3Checker();
            case 4:
                return Phase4Checker();
            case 5:
                return Phase5Checker();
            default:
                return true;
        }
    }

    public override void SettingRestore(int phase)
    {
        switch (phase)
        {
            case 2:
                Phase2Complete();
                break;
            case 3:
                Phase3Complete();
                break;
            case 4:
                Phase4Complete();
                break;
            case 5:
                Phase5Complete();
                break;
            default:
                return;
        }

        foreach (GameObject obj in restoreList)
            obj.SetActive(true);

        activeList.Clear();
        restoreList.Clear();
    }

    public bool Phase2Checker()
    {
        if (!phase2CheckerFlag1)
        {
            if (Tutorial.instance.dialogueUI.GetRemainSentencesCount() == 0)
            {
                RestoreChecker();
                phase2CheckerFlag1 = true;
            }
        }

        if (LevelManager.instance != null)
        {
            if (LevelManager.instance.isWaveProgress && !phase2CheckerFlag2)
            {
                BlockOneUI(UISpecific.WaveUI);
                phase2CheckerFlag2 = true;
            }
        }


        if (LevelManager.instance.enemyWaveList[0].oneWaveEnemies[0].transform.position.z < 2.6f)
        {
            Tutorial.instance.maskUI.SetActive(true);
            GameManager.instance.TimeStop();
            return true;
        }

        return false;
    }

    public void Phase2Complete()
    {
        foreach (AniObject ani in Tutorial.instance.tutorialList[3].maskAnimationList)
        {
            if (!ani.enable)
            {
                ani.posX = LevelManager.instance.enemyWaveList[0].oneWaveEnemies[0].transform.position.x;
                ani.posY = LevelManager.instance.enemyWaveList[0].oneWaveEnemies[0].transform.position.z;
            }
        }
    }

    public bool Phase3Checker()
    {
        if (!phase3CheckerFlag1)
        {
            if (Tutorial.instance.dialogueUI.GetRemainSentencesCount() == 0)
            {
                GameManager.instance.TimeRestore();
                phase3CheckerFlag1 = true;
            }
        }

        if (LevelManager.instance.waves.transform.GetChild(0).childCount == 0)
        {
            return true;
        }

        return false;
    }

    public void Phase3Complete()
    {
        RestoreOneUI(UISpecific.WaveUI);
        BlockChecker();
    }

    public bool Phase4Checker()
    {
        if (!phase4CheckerFlag1)
        {
            if (Tutorial.instance.dialogueUI.GetRemainSentencesCount() == 0)
            {
                RestoreChecker();
                phase4CheckerFlag1 = true;
            }
        }

        if (LevelManager.instance != null)
        {
            if (LevelManager.instance.isWaveProgress && !phase4CheckerFlag2)
            {
                BlockOneUI(UISpecific.WaveUI);
                phase4CheckerFlag2 = true;
            }
        }


        if (LevelManager.instance.enemyWaveList[1].oneWaveEnemies[9].transform.position.z < 2.6f)
        {
            Tutorial.instance.maskUI.SetActive(true);
            GameManager.instance.TimeStop();
            return true;
        }

        return false;
    }

    public void Phase4Complete()
    {
        foreach (AniObject ani in Tutorial.instance.tutorialList[5].maskAnimationList)
        {
            if (!ani.enable)
            {
                ani.posX = LevelManager.instance.enemyWaveList[1].oneWaveEnemies[9].transform.position.x;
                ani.posY = LevelManager.instance.enemyWaveList[1].oneWaveEnemies[9].transform.position.z;
            }
        }
    }

    public bool Phase5Checker()
    {
        if (!phase5CheckerFlag1)
        {
            if (Tutorial.instance.dialogueUI.GetRemainSentencesCount() == 0)
            {
                GameManager.instance.TimeRestore();
                phase5CheckerFlag1 = true;
            }
        }

        if (LevelManager.instance.waves.transform.GetChild(1).childCount == 0)
            return true;

        return false;
    }

    public void Phase5Complete()
    {
        RestoreOneUI(UISpecific.WaveUI);
        BlockChecker();
    }

}