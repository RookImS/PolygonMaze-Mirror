using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class testPrevious : MonoBehaviour
{
    public void previous()
    {
        GameManager.stack.Pop();
        SceneManager.LoadScene(GameManager.stack.Pop());

        //GameManager.queue.Clear();
    }
}
