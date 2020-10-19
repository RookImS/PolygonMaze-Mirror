using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class Dialogue : MonoBehaviour
{
    [Header("이름")]
    public string name;

    [Header("내용")]
    [TextArea(3, 10)]
    public List<string> sentences;

    private TextMeshProUGUI dialogueText;

    public void SetText(GameObject text)
    {
        dialogueText = text.GetComponent<TextMeshProUGUI>();
    }

    public int DisplayNextSentence()
    {
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentences[sentences.Count -1]));

        return sentences.Count;
    }




    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }
}
