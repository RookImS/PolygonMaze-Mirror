using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class JsonData
{
    public int m_nLevel;
    public Vector3 m_vecPosition;

    public void printData()
    {
        Debug.Log("Level : " + m_nLevel);
        Debug.Log("Position : " + m_vecPosition);
    }
}
public class JsonTest : MonoBehaviour
{
    void Start()
    {
        string str = File.ReadAllText(Application.dataPath + "/Json/TestJson.json");

        JsonData data = JsonUtility.FromJson<JsonData>(str);
        data.printData();
    }
}
