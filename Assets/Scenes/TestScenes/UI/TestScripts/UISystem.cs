using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UISystem : MonoBehaviour
{

    public EnemyData data;

    [Header("Player")]
    public Text CurrentCost;
    public Text CurrentHealth;
    public TextMeshProUGUI EnemyCount;
    public GameObject PlayerDefeatPanel;
    public GameObject EnemyHp;

    private void Awake()
    {
        PlayerControl.Instance.Init();
        //GameManager.Instance.Init();
    }

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.PlayerDeath += PlayerDead;
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePlayerUI();
    }

    void UpdatePlayerUI()
    {
        CurrentCost.text = PlayerControl.Instance.GetComponent<PlayerData>().cost.ToString();
        CurrentHealth.text = PlayerControl.Instance.GetComponent<PlayerData>().currentLife.ToString();
        //EnemyHealth.text = EnemyHealth.GetComponentInParent<EnemyData>().GetComponent<EnemyStatSystem>().currentHp.ToString();
        //EnemyCount.text = 50.ToString();
    }

    void Enemyhp()
    {
        //EnemyHp.GetComponent<TextMeshPro>().text = EnemyHp.GetComponentInParent<>
        //.text = Stats.currentHp.ToString();
    }
    void PlayerDead()
    {
        Transform parent = GameObject.Find("Canvas").GetComponent<Transform>();
        GameObject Temp = Instantiate(PlayerDefeatPanel);

        Temp.transform.SetParent(parent);
        Temp.transform.localPosition = Vector3.zero;
        Temp.transform.localScale = Vector3.one;
    }
}
