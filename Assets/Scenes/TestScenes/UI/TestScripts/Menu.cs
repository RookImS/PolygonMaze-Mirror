using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public string SceneToLoad;
    public Button Stage1Button;

    public void LoadGame()
    {
        SceneManager.LoadScene(SceneToLoad);
    }

    void Start()
    {
        //Debug.Log(Stage1Button.GetComponent<Image>().alphaHitTestMinimumThreshold);
        Stage1Button.GetComponent<Image>().alphaHitTestMinimumThreshold = 1f;
        //Stage1Button.GetComponent<>().
    }

}
