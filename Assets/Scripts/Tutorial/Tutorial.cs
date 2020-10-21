using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public GameObject inGameUI;
    public TutorialObject tutorial;
    public DialogueUI dialogueUI;

    private List<TutorialObject.Tutorial> tutorialList;
    private int phase;          // 튜토리얼 중 몇 번째 챕터인가를 나타냄
    private int phaseLength;
    private int chapterLength;  // 튜토리얼 현재 챕터의 길이
    private int chapterOrder;   // 튜토리얼 현재 챕터내에서 몇 번째 동작을 하는 중인지 나타냄

    public event Action nextTutorialChapter;

    private void Awake()
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
    private void Start()
    {
        phaseLength = tutorialList.Count;
        nextTutorialChapter?.Invoke();
    }

    private void StartPhase()
    {
        chapterLength = tutorialList[phase].dialogue.sentences.Count;
        dialogueUI.StartDialogue(tutorialList[phase].dialogue);
        InvokeNextTutorial();
    }

    public void InvokeNextTutorial()
    {
        if (dialogueUI.GetRemainSentencesCount() == 0)
        {
            dialogueUI.EndDialogue();

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
                        GameObject tempAniObject = Instantiate(ani.obj, inGameUI.transform);
                        tempAniObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(ani.posX, ani.posY);
                        StartCoroutine(DestroyAniObject(tempAniObject, chapterOrder + ani.length));

                        //aniObjectList.Add(tempAniObject);
                    }
                }
            }

        }
    }

    private IEnumerator PhaseChecker()
    {
        while (!tutorial.tutorialChecker.StartCheck(phase))
            yield return new WaitForSeconds(0.1f);

        phase++; 
        if (phase == phaseLength)
        {
            nextTutorialChapter -= StartPhase;
            // 게임시작 코드
        }

        nextTutorialChapter?.Invoke();
        yield return null;
    }

    private IEnumerator DestroyAniObject(GameObject aniObject, int endOrder)
    {
        while (!(dialogueUI.GetRemainSentencesCount() == 0 || chapterOrder >= endOrder))
            yield return new WaitForSecondsRealtime(0.1f);

        Destroy(aniObject);

        yield return null;
    }


}
