using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class DialogueUI : MonoBehaviour
{
    public Tutorial tutorial;

    public GameObject invokePanel;
    public GameObject textPanel;
    public GameObject TMProtext;

    private bool checkPrintSentence;
    [HideInInspector]public bool instancePrint;
    private bool isColor;
    private String colortag;
    private IEnumerator coroutine;

    private Dialogue m_dialogue;
    private GameObject m_textPanel;
    private GameObject m_text;
    private TextMeshProUGUI dialogueText;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        checkPrintSentence = false;
        isColor = false;
        instancePrint = false;
        m_dialogue = null;
        m_textPanel = null;
        m_text = null;
    }

    public void CleanDialogueUI()
    {
        StopAllCoroutines();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        m_dialogue = dialogue;
        invokePanel.SetActive(true);

        StartCoroutine(WaitInvokePanel());
        MakeDialoguePanel();
    }

    public void EndDialogue()
    {
        DestroyDialoguePanel();
        invokePanel.SetActive(false);
    }

    public void DisplayNextSentence()
    {
        if (checkPrintSentence)
        {
            checkPrintSentence = false;
            StopCoroutine(coroutine);
            PrintSentence();
        }
        else
        {
            checkPrintSentence = true;
            string arg_sentence = m_dialogue.sentences[0];

            coroutine = TypeSentence(arg_sentence);
            StartCoroutine(coroutine);
        }

        if (SoundManager.Instance != null)
            SoundManager.instance.PlaySound(SoundManager.SoundSpecific.OTHERUI, "Tutorial_Pass_Sound");
    }

    public int GetRemainSentencesCount()
    {
        return m_dialogue.sentences.Count;
    }
    private void MakeDialoguePanel()
    {
        m_textPanel = Instantiate(textPanel, invokePanel.transform);
        m_textPanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(m_dialogue.posX, m_dialogue.posY);
        m_textPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(m_dialogue.width, m_dialogue.height);

        m_text = Instantiate(TMProtext, m_textPanel.transform);
        m_text.GetComponent<RectTransform>().sizeDelta = new Vector2(m_dialogue.width - 100, m_dialogue.height - 50);
    }

    private void DestroyDialoguePanel()
    {
        Destroy(m_textPanel);
    }

    private void PrintSentence()
    {
        dialogueText.text = m_dialogue.sentences[0];
        m_dialogue.sentences.RemoveAt(0);
        instancePrint = true;
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText = m_text.GetComponent<TextMeshProUGUI>();
        dialogueText.text = "";
        instancePrint = false;

        foreach (char letter in sentence.ToCharArray())
        {
            if (letter.Equals('<') || isColor)
            {
                colortag += letter;
                if (letter.Equals('>'))
                {
                    dialogueText.text += colortag;
                    colortag = null;
                    isColor = false;
                    continue;
                }

                isColor = true;
                continue;
            }
            dialogueText.text += letter;
            yield return new WaitForSecondsRealtime(0.1f);
        }

        checkPrintSentence = false;
        m_dialogue.sentences.RemoveAt(0);
        yield return null;
    }

    IEnumerator WaitInvokePanel()
    {
        invokePanel.GetComponent<EventTrigger>().enabled = false;
        yield return new WaitForSecondsRealtime(0.5f);
        invokePanel.GetComponent<EventTrigger>().enabled = true;

        yield return null;
    }
}
