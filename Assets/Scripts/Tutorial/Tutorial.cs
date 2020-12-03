using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public GameObject aniUI;
    public GameObject maskUI;
    //public GameObject maskObject;
    public GameObject mask;
    public GameObject fieldAniUI;
    public TutorialObject tutorial;
    public DialogueUI dialogueUI;
    
    private List<TutorialObject.Tutorial> tutorialList;
    private TutorialChecker tutorialChecker;
    private int phase;          // 튜토리얼 중 몇 번째 챕터인가를 나타냄
    private int phaseLength;    // 튜토리얼이 몇 챕터로 이루어져있는지 나타냄
    private int chapterLength;  // 튜토리얼 현재 챕터의 길이
    private int chapterOrder;   // 튜토리얼 현재 챕터내에서 몇 번째 동작을 하는 중인지 나타냄
    public event Action nextTutorialChapter;

    private void Awake()
    {
        Init();
    }
    private void Start()
    {
        tutorialChecker = Instantiate(tutorial.tutorialChecker, this.transform).GetComponent<TutorialChecker>();
        phaseLength = tutorialList.Count;
        nextTutorialChapter?.Invoke();
    }

    private void Init()
    {
        phase = 0;
        phaseLength = 0;
        chapterLength = 0;
        chapterOrder = 0;
        tutorialList = new List<TutorialObject.Tutorial>();
        foreach (TutorialObject.Tutorial temp in tutorial.tutorialList)
            tutorialList.Add(new TutorialObject.Tutorial(temp));

        nextTutorialChapter += StartPhase;
    }
    private void StartPhase()
    {
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

            //tutorialList[phase].maskAnimationList.obj = mask;

            //if (!tutorialList[phase].maskAnimation.enable)
            //{
            //    if (tutorialList[phase].maskAnimation.order == chapterOrder)
            //    {
            //        maskUI.SetActive(true);
            //        tutorialList[phase].maskAnimation.enable = true;
            //        tutorialList[phase].maskAnimation.obj.transform.localPosition = new Vector2(tutorialList[phase].maskAnimation.posX, tutorialList[phase].maskAnimation.posY);
            //        tutorialList[phase].maskAnimation.obj.transform.localRotation = Quaternion.Euler(0f, 0f, tutorialList[phase].maskAnimation.rotation);
            //        //tutorialList[phase].maskAnimation.obj.SetActive(false);
            //    }
            //    else
            //        maskUI.SetActive(false);
            //}
            if(tutorialList[phase].maskAnimationList.Count == 0)
            {
                maskUI.SetActive(false);
            }
            foreach (AniObject ani in tutorialList[phase].maskAnimationList)
            {
                if (!ani.enable)
                {
                    if (ani.order == chapterOrder)
                    {
                        maskUI.SetActive(true);
                        ani.enable = true;
                        ani.obj = mask;
                        ani.obj.transform.localPosition = new Vector2(ani.posX, ani.posY);
                        ani.obj.transform.localRotation = Quaternion.Euler(0f, 0f, ani.rotation);
                    }
                }
            }
        }
    }

    private IEnumerator PhaseChecker()
    {
        while (!tutorialChecker.StartCheck(phase))
            yield return new WaitForSeconds(0.1f);

        tutorialChecker.SettingRestore(phase);

        phase++; 
        if (phase == phaseLength)
        {
            nextTutorialChapter -= StartPhase;
            LevelManager.instance.isWaveSystemOn = true;
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


}
