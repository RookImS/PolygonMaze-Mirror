using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class DeployUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject deployTower;
    public GameObject realTower;
    private GameObject newObject;
    private GameObject hitObject;

    private bool isDeployable;
    public TextMeshProUGUI towerCostText;

    Vector3 mousePos;

    private void Awake()
    {
        Init();
    }

    private void Update()
    {
        UpdateTowerCostText();
    }

    private void Init()
    {
        towerCostText.text = realTower.GetComponent<TowerData>().cost.ToString();
        isDeployable = false;
    }

    private void UpdateTowerCostText()
    {
        if (realTower.GetComponent<TowerData>().cost > PlayerControl.Instance.playerData.currentCost)
            isDeployable = false;
        else
            isDeployable = true;

        if (isDeployable)
            towerCostText.color = Color.white;
        else
            towerCostText.color = Color.red;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        mousePos = Input.mousePosition;

        newObject = Instantiate(deployTower, mousePos, deployTower.transform.rotation);

        Time.timeScale = 0.3f;
    }
    public void OnDrag(PointerEventData eventData)
    {
        mousePos = Input.mousePosition;

        hitObject = newObject.GetComponent<DeployBehaviour>().LocateTower(mousePos);
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        newObject.GetComponent<DeployBehaviour>().DeployTower(hitObject, realTower);

        Time.timeScale = 1f;
    }
}
