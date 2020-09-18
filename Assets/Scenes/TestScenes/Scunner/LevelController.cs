using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public Transform prefab;
    public void newObject()
    {
        Object.Instantiate(prefab, new Vector3(3.79f, 0.5f, -6.0f), Quaternion.identity);
    }
}
