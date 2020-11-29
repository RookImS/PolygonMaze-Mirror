using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{ 
    public static UIManager instance { get; private set; }
    public LayerMask towerMask;

    [HideInInspector] public CanvasGroup canvasGroup;

    [HideInInspector] public InfoUI infoUI;
    [HideInInspector] public bool isPanelOn;

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
        canvasGroup.blocksRaycasts = true;
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
        }
        else
        {
            infoUI.DisableInfo();
        }
    }
}
