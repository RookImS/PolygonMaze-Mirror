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
        BackKey.onClick.AddListener(PreviousScene);
    }
    public void PreviousScene()
    {
        GameManager.Instance.sceneStack.Pop();
        GameManager.Instance.LoadScene(GameManager.Instance.sceneStack.Pop());
    }
}
