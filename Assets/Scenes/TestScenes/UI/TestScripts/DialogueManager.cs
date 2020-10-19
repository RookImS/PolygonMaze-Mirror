using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoSingleton<DialogueManager>
{
    public TextMeshProUGUI dialogueText;

    public GameObject NextDialoguePanel;
    private Queue<string> sentences;
    
    // Start is called before the first frame update
    void Awake()
    {
        sentences = new Queue<string>();
    }

    public void StartDialogue (Dialogue dialogue)
    {
        NextDialoguePanel.SetActive(true);
        Debug.Log("Starting" + dialogue.name);

        sentences.Clear();

        foreach(string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if(sentences.Count == 0)
        {
            OnEndDialogue();
            return;
        }
        string sentence = sentences.Dequeue();
        //Coroutine a = StartCoroutine(TypeSentence(sentence));
        //StopCoroutine(a);
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }
    public event Action EndDialogue;
    public void OnEndDialogue()
    {
        Debug.Log("EndDialogue");
        NextDialoguePanel.SetActive(false);
        EndDialogue?.Invoke();
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach(char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }
}
