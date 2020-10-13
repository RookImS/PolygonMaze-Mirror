
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject[] objs;

    public void PauseSystem()
    {
        GameManager.Instance.TimeStop();
    }

    public void PlaybackSystem()
    {
        GameManager.Instance.TimeRestore();
    }

    public void MoveScene()
    {
        SceneManager.LoadScene("TestMainSceneUI");
    }

    public void Reset()
    {
        //PlayerControl.Instance.Init();
        objs = GameObject.FindGameObjectsWithTag("Enemy");
        foreach(GameObject obj in objs)
        {
            Destroy(obj);
        }

    }

}
