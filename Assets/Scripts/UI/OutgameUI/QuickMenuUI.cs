using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class QuickMenuUI : MonoBehaviour
{
    public Button BackKey;
    private void Awake()
    {
        BackKey.onClick.AddListener(() => GameManager.instance.LoadScene("MainScene"));
    }
}
