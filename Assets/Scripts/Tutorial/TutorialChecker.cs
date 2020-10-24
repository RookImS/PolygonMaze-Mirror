using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialChecker : MonoBehaviour
{
    [HideInInspector] public GameObject obstacles;
    [HideInInspector] public GameObject towers;
    [HideInInspector] public GameObject deployUI;
    [HideInInspector] public List<GameObject> activeList;
    [HideInInspector] public List<GameObject> restoreList;

    private void Start()
    {
        restoreList = new List<GameObject>();
        obstacles = GameObject.Find("Obstacles");
        towers = GameObject.Find("Towers");
        deployUI = GameObject.Find("DeployUI");
    }

    public void DisableSCExceptGameObject()
    {
        for (int i = 0; i < towers.transform.childCount; i++)
        {
            restoreList.Add(towers.transform.GetChild(i).Find("SideCollider").gameObject);
        }
        for (int i = 0; i < obstacles.transform.childCount; i++)
        {
            restoreList.Add(obstacles.transform.GetChild(i).Find("SideCollider").gameObject);
        }

        foreach (GameObject obj in activeList)
            restoreList.Remove(obj.transform.Find("SideCollider").gameObject);

        foreach (GameObject obj in restoreList)
            obj.SetActive(false);
    }

    public void DisableSCinExceptIndex(GameObject obj, int index)
    {
        Transform sc = obj.transform.Find("SideCollider");

        for (int i = 0; i < sc.childCount; i++)
        {
            restoreList.Add(sc.GetChild(i).gameObject);
        }

        restoreList.Remove(sc.GetChild(index).gameObject);
    }

    public void HighlightDeploySlot(int index)
    {
        for (int i = 0; i < deployUI.transform.childCount; i++)
        {
            if (i == index)
                deployUI.transform.GetChild(i).gameObject.GetComponent<Image>().color = Color.yellow;
            else
                deployUI.transform.GetChild(i).gameObject.GetComponent<DeployUI>().enabled = false;
        }
    }
    public void RestoreDeploySlot(int index)
    {
        for (int i = 0; i < deployUI.transform.childCount; i++)
        {
            if (i == index)
                deployUI.transform.GetChild(i).gameObject.GetComponent<Image>().color = Color.white;
            else
                deployUI.transform.GetChild(i).gameObject.GetComponent<DeployUI>().enabled = true;
        }
    }
    public virtual void StartSetting(int phase)
    {

    }
    public virtual bool StartCheck(int phase)
    {
        return false;
    }
    public virtual void SettingRestore(int phase)
    {

    }
}
