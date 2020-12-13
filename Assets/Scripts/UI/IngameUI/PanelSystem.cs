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
        UIManager.instance.isPanelOn = true;
        if (SoundManager.instance != null)
            SoundManager.Instance.PlaySound(SoundManager.SoundSpecific.BUTTON, "Common_Button");
    }
    public void Resume()
    {
        backGroundPanel.SetActive(false);
        pausePanel.SetActive(false);
        GameManager.Instance.TimeRestore();
        UIManager.instance.isPanelOn = false;
        if (SoundManager.instance != null)
            SoundManager.Instance.PlaySound(SoundManager.SoundSpecific.BUTTON, "Common_Button");
    }
    public void ReturnToMain()
    {
        GameManager.Instance.LoadScene("MainScene");
        GameManager.Instance.TimeRestore();
        if (SoundManager.instance != null)
            SoundManager.Instance.PlaySound(SoundManager.SoundSpecific.BUTTON, "Common_Button");
        Tutorial.instance.CleanTutorial();
    }

    public void RestartGame()
    {
        GameManager.Instance.LoadScene(SceneManager.GetActiveScene().name);
        GameManager.Instance.TimeRestore();
        if (SoundManager.instance != null)
            SoundManager.Instance.PlaySound(SoundManager.SoundSpecific.BUTTON, "Common_Button");
        Tutorial.instance.CleanTutorial();
    }
}
