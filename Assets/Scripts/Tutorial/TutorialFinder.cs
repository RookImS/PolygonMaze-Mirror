using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialFinder : MonoBehaviour
{
    private void Start()
    {
        Tutorial.Instance.aniUI.GetComponent<Canvas>().worldCamera = GetComponent<Camera>();
        Tutorial.Instance.fieldAniUI.GetComponent<Canvas>().worldCamera = GetComponent<Camera>();
        Tutorial.Instance.dialogueUI.gameObject.GetComponent<Canvas>().worldCamera = GetComponent<Camera>();
    }
}
