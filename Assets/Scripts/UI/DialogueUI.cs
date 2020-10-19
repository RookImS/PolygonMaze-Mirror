using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueUI : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public GameObject invokePanel;
    public GameObject textPanel;
    public GameObject TMProtext;

    private GameObject m_textPanel;
    private GameObject m_text;


    private void Awake()
    {

    }

    public void StartDialogue(Dialogue dialogue, int posx, int posy)
    {
        invokePanel.SetActive(true);

        MakeDialoguePanel(posx, posy);
        dialogue.SetText(m_text);
    }

    private void MakeDialoguePanel(int posx, int posy)
    {
        m_textPanel = Instantiate(textPanel);
        m_textPanel.transform.SetParent(this.transform);
        m_textPanel.transform.localPosition = new Vector3(posx, posy, 0);
        m_textPanel.transform.localScale = Vector3.one;

        m_text = Instantiate(TMProtext);
        m_text.transform.SetParent(m_textPanel.transform);
        m_text.transform.localPosition = new Vector3(0, 0, 0);
        m_text.transform.localScale = Vector3.one;
    }

    private void DestroyDialoguePanel()
    {
        Destroy(m_textPanel);
    }
}
