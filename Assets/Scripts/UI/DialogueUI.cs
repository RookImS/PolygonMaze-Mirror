using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueUI : MonoBehaviour
{
    public GameObject invokePanel;
    public GameObject textPanel;
    public GameObject TMProtext;

    private bool checkPrintSentence;
    private Dialogue m_dialogue;
    private GameObject m_textPanel;
    private GameObject m_text;

    private void Awake()
    {
        checkPrintSentence = false;
        m_dialogue = null;
        m_textPanel = null;
        m_text = null;
    }

    public void StartDialogue(Dialogue dialogue)
    {
        m_dialogue = dialogue;
        invokePanel.SetActive(true);

        GameManager.Instance.TimeStop();
        MakeDialoguePanel();
    }

    public void EndDialogue()
    {
        DestroyDialoguePanel();
        GameManager.Instance.TimeRestore();

        invokePanel.SetActive(false);
    }

    public void DisplayNextSentence()
    {
        if (checkPrintSentence)
        {
            checkPrintSentence = false;
        }
        else
        {
            checkPrintSentence = true;

            string arg_sentence = m_dialogue.sentences[0];
            StopAllCoroutines();
            StartCoroutine(TypeSentence(arg_sentence));
        }
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



    IEnumerator TypeSentence(string sentence)
    {
        TextMeshProUGUI dialogueText = m_text.GetComponent<TextMeshProUGUI>();
        dialogueText.text = "";

        foreach (char letter in sentence.ToCharArray())
        {
            if (checkPrintSentence)
            {
                dialogueText.text += letter;
                yield return new WaitForSecondsRealtime(0.1f);
            }
            else
            {
                dialogueText.text = sentence;
                break;
            }
        }

        checkPrintSentence = false;
        m_dialogue.sentences.RemoveAt(0);
        yield return null;
    }
}
