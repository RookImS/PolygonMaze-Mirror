using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;
using UnityEngine.UI;
public class UISystem : MonoBehaviour
{
    public static UISystem Instance { get; private set; }

    [Header("Player")]
    public Text CurrentCost;
    public Text CurrentHealth;

    private void Awake()
    {
        Instance = this;
        PlayerControl.Instance.Init();
        GameManager.Instance.Init();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePlayerUI();
    }

    void UpdatePlayerUI()
    {
        CurrentCost.text = PlayerControl.Instance.GetComponent<PlayerData>().cost.ToString();
        CurrentHealth.text = PlayerControl.Instance.GetComponent<PlayerData>().life.ToString();
    }
}
