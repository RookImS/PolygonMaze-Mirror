using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public GameObject menu;
    public string sceneToLoad;
    public Button stageButton;

    public void LoadGame()
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    public void OnClickCancleButton()
    {
        GameManager.Instance.SetLoadStageChapter(0);
        GameManager.Instance.SetLoadStageLevel(0);
        menu.SetActive(false);
    }

    void Start()
    {
        //Debug.Log(Stage1Button.GetComponent<Image>().alphaHitTestMinimumThreshold);
        //stageButton.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.1f;
        //Stage1Button.GetComponent<>().
    }

}
