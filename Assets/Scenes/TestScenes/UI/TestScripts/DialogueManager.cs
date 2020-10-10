using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class DialogueManager : MonoSingleton<DialogueManager>
{
    public TextMeshProUGUI name;
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
            EndDialogue();
            return;
        }
        string sentence = sentences.Dequeue();

        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));

    }

    void EndDialogue()
    {
        Debug.Log("EndDialogue");
        TutorialManager.Instance.OnEndDialogue();
    }
    // Update is called once per frame
    void Update()
    {
        
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
