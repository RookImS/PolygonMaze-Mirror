using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    private void Start()
    {
        DialogueManager.Instance.EndDialogue += Disable;
    }
    public void TriggerDialogue()
    {
        gameObject.SetActive(true);
        DialogueManager.Instance.StartDialogue(dialogue);
    }

    void Disable()
    {
        gameObject.SetActive(false);
    }

}
