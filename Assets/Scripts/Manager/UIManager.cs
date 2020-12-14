using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIManager : MonoBehaviour
{ 
    public static UIManager instance { get; private set; }
    public LayerMask towerMask;
    public Animator flickerEffect;
    public TextMeshProUGUI currentwaveText;
    public TextMeshProUGUI MaxwaveText;
    public Button currentwaveButton;

    [HideInInspector] public InfoUI infoUI;
    [HideInInspector] public bool isPanelOn;

    private CanvasGroup canvasGroup;

    private void Awake()
    {
        
        instance = this;
        isPanelOn = false;
    }

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        BlockRaycastOn();
        infoUI = GameObject.Find("InfoUI").GetComponent<InfoUI>();
        MaxwaveText.text = "Max waves : " + LevelManager.instance.waves.transform.childCount.ToString();
        WaveUI();
    }

    public void OnClick()
    {
        Vector3 realPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (UIManager.instance.isPanelOn)
        {
            return;
        }

        ControlTowerInfo(ray);
    }

    public void WaveUI()
    {
        currentwaveText.text = "Wave" + (LevelManager.instance.currentWave+1).ToString() + "\nStart";
        if(LevelManager.instance.currentWave + 1 > LevelManager.instance.waves.transform.childCount)
        {
            currentwaveText.text = "Wave\nEND";
            currentwaveButton.interactable = false;
        }
        currentwaveText.text.Replace("\\n", "\n");
    }

    public void BlockRaycastOn()
    {
        canvasGroup.blocksRaycasts = true;
    }

    public void BlockRaycastOff()
    {
        canvasGroup.blocksRaycasts = false;
    }

    private void ControlTowerInfo(Ray ray)
    {
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Camera.main.transform.position.y * 1.3f, towerMask))
        {
            GameObject hitTower = hit.collider.gameObject;

            if (hitTower.GetComponent<TowerBehaviour>().isClicked)
            {
                infoUI.DisableInfo();
            }
            else
            {
                infoUI.EnableInfo(hitTower);
            }

            if (SoundManager.instance != null)
                SoundManager.instance.PlaySound(SoundManager.SoundSpecific.OTHERUI, "Tower_Click_Sound");
        }
        else
        {
            infoUI.DisableInfo();
        }
    }

    
}
