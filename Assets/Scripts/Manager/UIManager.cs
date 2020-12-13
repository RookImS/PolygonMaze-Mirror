using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{ 
    public static UIManager instance { get; private set; }
    public LayerMask towerMask;

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
