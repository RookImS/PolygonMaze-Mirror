using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "Dialogue01", menuName = "ScriptableObjects/DialogueScriptableObject", order = 2)]
public class DialogueScriptableObject : ScriptableObject
{
    //[Serializable]
    //public class Dialogue
    //{
    //    [Header("이름")]
    //    public string name;

    //    [Header("내용")]
    //    [TextArea(3, 10)]
    //    public string[] sentences;
    //}

    [Header("안내문 리스트")]
    
    public List<Dialogue> dialogueList;
}
