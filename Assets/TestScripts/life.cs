using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class life : MonoBehaviour
{
    private Text ScriptTxt;
    public float CurrentHealth;
    private void Start()
    {
        ScriptTxt = GetComponentInChildren<Text>();
        ScriptTxt.text = CurrentHealth.ToString();
    }
}
