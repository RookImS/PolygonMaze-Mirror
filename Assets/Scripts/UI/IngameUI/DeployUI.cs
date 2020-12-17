using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class DeployUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler
{
    public GameObject deployTower;
    public GameObject realTower;

    private GameObject newObject;
    [HideInInspector] public GameObject hitObject;

    private bool isDeployable;
    public TextMeshProUGUI towerCostText;

    [HideInInspector] public bool isProgressDeploy;
    [HideInInspector] public bool isActive;

    private Vector3 mousePos;

    private void Awake()
    {
        Init();
    }

    private void Update()
    {
        UpdateTowerCostText();
        if (!isActive || GameManager.Instance.isGameOver || GameManager.Instance.isStageClear)
        {
            if (newObject != null)
                Destroy(newObject);
        }
    }

    private void Init()
    {
        towerCostText.text = deployTower.GetComponent<DeployBehaviour>().cost.ToString();
        isDeployable = false;
        isProgressDeploy = false;
        isActive = true;
    }

    private void UpdateTowerCostText()
    {
        if (PlayerControl.Instance.CheckCost(deployTower.GetComponent<DeployBehaviour>().cost))
            isDeployable = true;
        else
            isDeployable = false;

        if (isDeployable)
            towerCostText.color = Color.white;
        else
            towerCostText.color = Color.red;
    }

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        if(isActive)
            SoundManager.Instance.PlaySound(SoundManager.SoundSpecific.BUTTON, "Tower_Button");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isActive)
        {
            newObject = null;
            hitObject = null;
            UIManager.instance.infoUI.DisableInfo();
            UIManager.instance.BlockRaycastOff();

            isProgressDeploy = true;
            mousePos = Input.mousePosition;

            newObject = Instantiate(deployTower, mousePos, deployTower.transform.rotation);

            if(Tutorial.instance.tutorialChecker == null && Time.timeScale != 0f)
                GameManager.instance.SlowTime(0.3f);
        }
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (isActive && !GameManager.Instance.isGameOver && !GameManager.Instance.isStageClear)
        {
            mousePos = Input.mousePosition;

            hitObject = newObject.GetComponent<DeployBehaviour>().LocateTower(mousePos);
        }
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (isActive && !GameManager.Instance.isGameOver && !GameManager.Instance.isStageClear)
        {
            newObject.GetComponent<DeployBehaviour>().DeployTower(hitObject, realTower);

            if (Tutorial.instance.tutorialChecker == null && Time.timeScale != 0f)
                GameManager.instance.TimeRestore();
            
            isProgressDeploy = false;
            UIManager.instance.BlockRaycastOn();
            SoundManager.Instance.PlaySound(SoundManager.SoundSpecific.OTHERUI, "Tower_Locate_Sound");
        }
    }
}
