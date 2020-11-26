using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public string sceneToLoad;
    public Button stageButton;
    public int stageChapter;
    public int stageLevel;

    public void LoadGame()
    {
        GameManager.Instance.SetLoadStageChapter(this.stageChapter);
        GameManager.Instance.SetLoadStageLevel(this.stageLevel);
        SceneManager.LoadScene(sceneToLoad);
    }

    void Start()
    {
        //Debug.Log(Stage1Button.GetComponent<Image>().alphaHitTestMinimumThreshold);
        //stageButton.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.1f;
        //Stage1Button.GetComponent<>().
    }

}
