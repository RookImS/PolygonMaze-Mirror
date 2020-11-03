using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class Dialogue
{
    public Dialogue(Dialogue d)
    {
        name = d.name;

        sentences = new List<string>();
        foreach (string temp in d.sentences)
            sentences.Add(string.Copy(temp));

        posX = d.posX;
        posY = d.posY;
        width = d.width;
        height = d.height;
    }

    [Header("이름")]
    public string name;

    [Header("내용")]
    [TextArea(3, 10)]
    public List<string> sentences;

    [Header("생성위치")]
    [SerializeField]
    public float posX;
    public float posY;

    [Header("생성 크기")]
    [SerializeField]
    public float width = 1000;
    public float height = 300;
}
