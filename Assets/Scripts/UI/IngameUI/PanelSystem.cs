using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PanelSystem : MonoBehaviour
{
    public GameObject backGroundPanel;
    public GameObject pausePanel;
    public GameObject stageClearPanel;
    public GameObject gameOverPanel;

    public void SetPanel(GameObject panel)
    {
        backGroundPanel.SetActive(true);
        panel.SetActive(true);
        GameManager.Instance.TimeStop();
    }
    public void Resume()
    {
        backGroundPanel.SetActive(false);
        pausePanel.SetActive(false);
        GameManager.Instance.TimeRestore();
    }
    public void ReturnToMain()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
