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
    private Dialogue m_dialogue;
    private GameObject m_textPanel;
    private GameObject m_text;

    private EventTrigger eventTrigger;
    private EventTrigger.Entry entry;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        checkPrintSentence = false;
        m_dialogue = null;
        m_textPanel = null;
        m_text = null;

        eventTrigger = invokePanel.GetComponent<EventTrigger>();

        entry = new EventTrigger.Entry();
        entry.callback.AddListener((eventData) => tutorial.InvokeNextTutorial());
        entry.eventID = EventTriggerType.PointerClick;
    }

    public void StartDialogue(Dialogue dialogue)
    {
        m_dialogue = dialogue;
        invokePanel.SetActive(true);

        StartCoroutine(AddInvokeTrigger());
        GameManager.Instance.TimeStop();
        MakeDialoguePanel();
    }

    public void EndDialogue()
    {
        DestroyDialoguePanel();

        eventTrigger.triggers.Remove(entry);

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

            StartCoroutine(TypeSentence(arg_sentence));
        }

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

    IEnumerator AddInvokeTrigger()
    {
        yield return new WaitForSecondsRealtime(0.7f);

        eventTrigger.triggers.Add(entry);

        yield return null;
    }
}