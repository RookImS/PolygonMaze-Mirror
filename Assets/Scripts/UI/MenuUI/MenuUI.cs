using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    public string sceneToLoad;

    public void SelectMenu()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
