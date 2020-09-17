using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    // Start is called before the first frame update

    public void PauseSystem()
    {
        Time.timeScale = 0f;
    }

    public void PlaybackSystem()
    {
        Time.timeScale = 1f;
    }

    public void MoveScene()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
