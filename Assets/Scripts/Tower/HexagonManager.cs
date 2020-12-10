using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexagonManager : MonoBehaviour
{
    public static HexagonManager instance { get; private set; }
    public List<HexagonBehaviour> HexagonList;

    private void Awake()
    {
        instance = this;
        HexagonList = new List<HexagonBehaviour>();
    }
}
