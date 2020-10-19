
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseUI : MonoBehaviour
{
    public GameObject backgroundPanel;
    public GameObject pausePanel;

    public void Pause()
    {
        backgroundPanel.SetActive(true);
        pausePanel.SetActive(true);
        GameManager.Instance.TimeStop();
    }
}
