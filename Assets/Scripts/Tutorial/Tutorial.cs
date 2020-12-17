using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoSingleton<Tutorial>
{
    public GameObject aniUI;
    public GameObject maskUI;
    //public GameObject mask;
    public GameObject fieldAniUI;
    [HideInInspector] public TutorialObject tutorial;
    public DialogueUI dialogueUI;

    [HideInInspector] public List<TutorialObject.Tutorial> tutorialList;
    [HideInInspector] public TutorialChecker tutorialChecker;
    private int phase;          // 튜토리얼 중 몇 번째 챕터인가를 나타냄
    private int phaseLength;    // 튜토리얼이 몇 챕터로 이루어져있는지 나타냄
    private int chapterLength;  // 튜토리얼 현재 챕터의 길이
    [HideInInspector] int chapterOrder;   // 튜토리얼 현재 챕터내에서 몇 번째 동작을 하는 중인지 나타냄
    public event Action nextTutorialChapter;

    public void CleanTutorial()
    {
        tutorialList = null;
        for (int i = 0; i < fieldAniUI.transform.childCount; i++)
            Destroy(fieldAniUI.transform.GetChild(i).gameObject);

        for (int i = 0; i < dialogueUI.invokePanel.transform.childCount; i++)
            Destroy(dialogueUI.invokePanel.transform.GetChild(i).gameObject);

        nextTutorialChapter -= StartPhase;
        //maskUI.SetActive(false);

        if (tutorialChecker != null)
        {
            Destroy(tutorialChecker.gameObject);
            tutorialChecker = null;
        }

        dialogueUI.CleanDialogueUI();
        StopAllCoroutines();
    }

    public void StartTutorial()
    {
        phase = 0;
        phaseLength = 0;
        chapterLength = 0;
        chapterOrder = 0;
        tutorialList = new List<TutorialObject.Tutorial>();
        foreach (TutorialObject.Tutorial temp in tutorial.tutorialList)
            tutorialList.Add(new TutorialObject.Tutorial(temp));

        nextTutorialChapter += StartPhase;

        tutorialChecker = Instantiate(tutorial.tutorialChecker, this.transform).GetComponent<TutorialChecker>();
        phaseLength = tutorialList.Count;
        nextTutorialChapter?.Invoke();
    }

    private void StartPhase()
    {
        if (LevelManager.instance != null)
            LevelManager.instance.isWaveSystemOn = false;
        chapterLength = tutorialList[phase].dialogue.sentences.Count;
        dialogueUI.StartDialogue(tutorialList[phase].dialogue);
        InvokeNextTutorial();
    }

    public void InvokeNextTutorial()
    {
        if (dialogueUI.GetRemainSentencesCount() == 0)
        {
            dialogueUI.EndDialogue();
            chapterOrder = chapterLength - dialogueUI.GetRemainSentencesCount() + 1;
            tutorialChecker.StartSetting(phase);
            StartCoroutine(PhaseChecker());
        }
        else
        {
            dialogueUI.DisplayNextSentence();

            if (dialogueUI.instancePrint)
                chapterOrder = chapterLength - dialogueUI.GetRemainSentencesCount();
            else
                chapterOrder = chapterLength - dialogueUI.GetRemainSentencesCount() + 1;

            foreach (AniObject ani in tutorialList[phase].animationList)
            {
                if (!ani.enable)
                {
                    if (ani.order == chapterOrder)
                    {
                        ani.enable = true;
                        GameObject tempAniObject = Instantiate(ani.obj, aniUI.transform);
                        //tempAniObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(ani.posX, ani.posY);
                        //tempAniObject.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0f, 0f, ani.rotation);
                        tempAniObject.transform.position = new Vector3(ani.posX, 10, ani.posY);
                        tempAniObject.transform.localRotation = Quaternion.Euler(90f, 0f, ani.rotation);
                        StartCoroutine(DestroyAniObject(tempAniObject, chapterOrder + ani.length));
                    }
                }
            }

            foreach (AniObject ani in tutorialList[phase].fieldAnimationList)
            {
                if (!ani.enable)
                {
                    if (ani.order == chapterOrder)
                    {
                        ani.enable = true;
                        GameObject tempAniObject = Instantiate(ani.obj, fieldAniUI.transform);
                        tempAniObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(ani.posX, ani.posY);
                        tempAniObject.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0f, 0f, ani.rotation);
                        StartCoroutine(DestroyAniObject(tempAniObject, chapterOrder + ani.length));
                    }
                }
            }

           
            foreach (AniObject ani in tutorialList[phase].maskAnimationList)
            {
                if (!ani.enable)
                {
                    if (ani.order == chapterOrder)
                    {
                        if (ani.isMaskOn)
                            maskUI.SetActive(true);
                        ani.enable = true;
  
                        GameObject tempAniObject = Instantiate(ani.obj, maskUI.transform);
                        tempAniObject.transform.localPosition = new Vector2(ani.posX, ani.posY);
                        tempAniObject.transform.localRotation = Quaternion.Euler(0f, 0f, ani.rotation);
                        StartCoroutine(DestoryMaskObject(tempAniObject, chapterOrder + ani.length, ani.isMaskOff));
                    }
                }

                
            }
        }
    }

    private IEnumerator PhaseChecker()
    {
        if (tutorialChecker == null)
            yield return null;

        while (!tutorialChecker.StartCheck(phase))
            yield return null;

        tutorialChecker.SettingRestore(phase);

        phase++; 
        if (phase == phaseLength)
        {
            nextTutorialChapter -= StartPhase;
            if(LevelManager.instance != null)
                LevelManager.instance.isWaveSystemOn = true;

            CleanTutorial();
        }

        nextTutorialChapter?.Invoke();
        yield return null;
    }

    private IEnumerator DestroyAniObject(GameObject aniObject, int endOrder)
    {
        int appearPhase = phase;    // 애니메이션 실행된 phase

        while (!(appearPhase != phase || chapterOrder >= endOrder))
            yield return new WaitForSecondsRealtime(0.1f);

        Destroy(aniObject);

        yield return null;
    }

    private IEnumerator DestoryMaskObject(GameObject aniObject, int endOrder, bool isMaskOff)
    {
        int appearPhase = phase;    // 애니메이션 실행된 phase

        while (!(appearPhase != phase || chapterOrder >= endOrder))
        {
            yield return new WaitForSecondsRealtime(0.1f);
        }

        if (isMaskOff)
            maskUI.SetActive(false);

        Destroy(aniObject);

        yield return null;
    }
}
