using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PanelUISystem : MonoSingleton<PanelUISystem>
{
    public GameObject BackGroundPanel;
    public GameObject PausePanel;
    public GameObject GameClearPanel;
    public GameObject GameOverPanel;

    public void ReturnToMain()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
