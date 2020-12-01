using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    public string sceneToLoad;

    private void Start()
    {
        gameObject.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.1f;
    }

    public void SelectMenu()
    {
        GameManager.Instance.LoadScene(sceneToLoad);
    }
}
